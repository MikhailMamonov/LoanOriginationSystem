using LOS.Domain.Clients;
using LOS.Domain.Common.Interfaces;
using LOS.Domain.Common.Interfaces.Repositories;
using MediatR;

namespace LOS.Application.Clients.Commands.CreateClient;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Guid>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateClientCommand request, CancellationToken ct)
    {
        // Доменная модель сама проверит инварианты (возраст, формат email, паспорта и т.д.)
        var client = new Client(
            firstName: request.FirstName,
            lastName: request.LastName,
            email: request.Email,
            phone: request.Phone,
            birthDate: request.BirthDate,
            passportSeries: request.PassportSeries,
            passportNumber: request.PassportNumber);

        await _clientRepository.AddAsync(client, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return client.Id;
    }
}