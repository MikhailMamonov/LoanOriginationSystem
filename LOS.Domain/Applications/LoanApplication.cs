using LOS.Domain.Common.Exceptions;
using LOS.Domain.Common;
namespace LOS.Domain.LoanApplications;


public class LoanApplication : AggregateRoot
{
    public Guid ClientId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal RequestedAmount { get; private set; }
    public int RequestedTermMonths { get; private set; }
    public ApplicationStatus Status { get; private set; }

    public ICollection<ApplicationTimelineEntry> Timeline { get; private set; }

    private LoanApplication() { }

    public LoanApplication(Guid clientId, Guid productId, decimal amount, int term)
    {
        Id = Guid.NewGuid();
        ClientId = clientId;
        ProductId = productId;
        RequestedAmount = amount;
        RequestedTermMonths = term;
        Status = ApplicationStatus.Draft;

        Timeline = new List<ApplicationTimelineEntry>
        {
            new ApplicationTimelineEntry(
                fromStatus: null,
                toStatus: Status,
                changedBy: "system",
                changedAt: DateTime.UtcNow)
        };
    }

    public void Submit()
    {
        if (Status != ApplicationStatus.Draft)
            throw new InvalidEntityStateException(
                entityName: nameof(LoanApplication),
                entityId: Id,
                message: $"Невозможно подать заявку в статусе {Status}. Ожидается статус Draft.",
                currentState: Status.ToString());

        Status = ApplicationStatus.Submitted;
        Timeline.Add(new ApplicationTimelineEntry(
            fromStatus: ApplicationStatus.Draft,
            toStatus: ApplicationStatus.Submitted,
            changedBy: "client",
            changedAt: DateTime.UtcNow));
    }

    public void StartScoring()
    {
        if (Status != ApplicationStatus.Submitted)
            throw new InvalidEntityStateException(
                entityName: nameof(LoanApplication),
                entityId: Id,
                message: $"Невозможно начать скоринг в статусе {Status}. Ожидается статус Submitted.",
                currentState: Status.ToString());

        Status = ApplicationStatus.ScoringInProgress;
        // Добавляем запись в Timeline
    }

    public void CompleteScoring(decimal score)
    {
        if (Status != ApplicationStatus.ScoringInProgress)
            throw new InvalidEntityStateException(
                entityName: nameof(LoanApplication),
                entityId: Id,
                message: $"Невозможно завершить скоринг в статусе {Status}. Ожидается статус ScoringInProgress.",
                currentState: Status.ToString());

        // Пример простого скоринга
        if (score >= 80)
        {
            Status = ApplicationStatus.Approved;
        }
        else if (score >= 60)
        {
            Status = ApplicationStatus.ManualReview;
        }
        else
        {
            Status = ApplicationStatus.Rejected;
        }
    }
}