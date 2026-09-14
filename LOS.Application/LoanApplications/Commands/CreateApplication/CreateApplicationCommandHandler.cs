using LOS.Domain.Common.Interfaces.Repositories;
using LOS.Domain.Common.Interfaces;
using LOS.Domain.Common.Exceptions;
using LOS.Domain.Clients;
using LOS.Domain.LoanProducts;
using LOS.Domain.LoanApplications;
using MediatR;


namespace LOS.Application.LoanApplications.Commands.CreateApplication;

/// <summary>
/// Обработчик команды CreateApplicationCommand
/// Содержит бизнес-оркестрацию: загрузка агрегатов, проверка бизнес-правил, создание новой заявки
/// </summary>
public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, Guid>
{
    private readonly IClientRepository _clientRepository;
    private readonly ILoanProductRepository _productRepository;
    private readonly ILoanApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateApplicationCommandHandler(
        IClientRepository clientRepository,
        ILoanProductRepository productRepository,
        ILoanApplicationRepository applicationRepository,
        IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _productRepository = productRepository;
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken ct)
    {
        var client = await _clientRepository.GetByIdAsync(request.ClientId, ct);
        var product = await _productRepository.GetByIdAsync(request.ProductId, ct);

        // 2. Проверка существования
        if (client == null)
            throw new EntityNotFoundException(nameof(Client), request.ClientId);

        if (product == null)
            throw new EntityNotFoundException(nameof(LoanProduct), request.ProductId);


        // 3. Проверка бизнес-правил
        if (!product.IsClientEligible(client))
            throw new BusinessRuleViolationException(
                "ClientNotEligible",
                $"Клиент {client.Id} не соответствует требованиям продукта {product.Code}");

        // 4. Создание агрегата
        var application = new LoanApplication(
            clientId: client.Id,
            productId: product.Id,
            amount: request.RequestedAmount,
            term: request.RequestedTermMonths);

        // 5. Сохранение
        await _applicationRepository.AddAsync(application, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        // 6. Возврат результата
        return application.Id;
    }
}