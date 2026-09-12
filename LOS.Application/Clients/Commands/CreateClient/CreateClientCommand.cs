using MediatR;

namespace LOS.Application.Clients.Commands.CreateClient;

public record CreateClientCommand(
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    DateTime BirthDate,
    string PassportSeries,
    string PassportNumber) : IRequest<Guid>;
