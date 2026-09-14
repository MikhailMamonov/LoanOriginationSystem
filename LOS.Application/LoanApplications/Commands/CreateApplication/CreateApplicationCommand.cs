using MediatR;

namespace LOS.Application.LoanApplications.Commands.CreateApplication;

/// <summary>
/// Команда для создания заявки на кредит
/// </summary>
public record CreateApplicationCommand(
    Guid ClientId,
    Guid ProductId,
    decimal RequestedAmount,
    int RequestedTermMonths) : IRequest<Guid>;