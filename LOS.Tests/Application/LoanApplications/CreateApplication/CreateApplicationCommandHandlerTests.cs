using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LOS.Application.LoanApplications.Commands.CreateApplication;
using LOS.Domain.Common.Exceptions;
using LOS.Domain.Common.Interfaces;
using LOS.Domain.Common.Interfaces.Repositories;
using LOS.Domain.Clients;
using LOS.Domain.LoanProducts;
using LOS.Domain.LoanApplications;
using Moq;
using Xunit;

namespace LOS.Tests.Application.LoanApplications.CreateApplication;

public class CreateApplicationCommandHandlerTests
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<ILoanProductRepository> _productRepositoryMock;
    private readonly Mock<ILoanApplicationRepository> _applicationRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly CreateApplicationCommandHandler _handler;

    public CreateApplicationCommandHandlerTests()
    {
        // Инициализируем моки перед каждым тестом
        _clientRepositoryMock = new Mock<IClientRepository>();
        _productRepositoryMock = new Mock<ILoanProductRepository>();
        _applicationRepositoryMock = new Mock<ILoanApplicationRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        // Создаем экземпляр хендлера с моками
        _handler = new CreateApplicationCommandHandler(
            _clientRepositoryMock.Object,
            _productRepositoryMock.Object,
            _applicationRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WhenClientNotFound_ShouldThrowEntityNotFoundException()
    {
        // 1. Arrange (Подготовка)
        var clientId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CreateApplicationCommand(
            ClientId: clientId,
            ProductId: productId,
            RequestedAmount: 100000,
            RequestedTermMonths: 12);

        // Настраиваем мок: при запросе клиента с таким ID возвращаем null
        _clientRepositoryMock
            .Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Client?)null);

        // 2. Act (Действие)
        // Оборачиваем вызов в Action, чтобы перехватить исключение
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // 3. Assert (Проверка)
        await act.Should().ThrowAsync<EntityNotFoundException>()
            .WithMessage($"Сущность 'Client' с ID {clientId} не найдена.");

        // Проверяем, что репозиторий заявок и UnitOfWork НЕ были вызваны
        _applicationRepositoryMock.Verify(r => r.AddAsync(It.IsAny<LoanApplication>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenClientNotEligible_ShouldThrowBusinessRuleViolationException()
    {
        // 1. Arrange
        var clientId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var command = new CreateApplicationCommand(clientId, productId, 100000, 12);

        // Создаем фейкового клиента (слишком молодого)
        var youngClient = new Client(
            firstName: "Иван",
            lastName: "Иванов",
            email: "ivan@test.com",
            phone: "+79991234567",
            birthDate: DateTime.Today.AddYears(-17), // 17 лет
            passportSeries: "1234",
            passportNumber: "567890");

        // Создаем фейковый продукт (требует от 18 лет)
        var product = new LoanProduct(
            code: "TEST_PROD",
            name: "Тестовый продукт",
            description: "Описание",
            baseInterestRate: 10m,
            minAmount: 50000,
            maxAmount: 1000000,
            minTermMonths: 6,
            maxTermMonths: 60,
            fees: new ProductFees(0, 0, false),
            eligibility: new EligibilityCriteria(18, 65, 30000, 3, "RU", 0.5m));

        _clientRepositoryMock.Setup(r => r.GetByIdAsync(clientId, It.IsAny<CancellationToken>())).ReturnsAsync(youngClient);
        _productRepositoryMock.Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>())).ReturnsAsync(product);

        // 2. Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // 3. Assert
        await act.Should().ThrowAsync<BusinessRuleViolationException>()
            .Where(ex => ex.RuleName == "ClientNotEligible");

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}