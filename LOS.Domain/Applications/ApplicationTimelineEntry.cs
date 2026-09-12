namespace LOS.Domain.LoanApplications;

public class ApplicationTimelineEntry
{
    public Guid Id { get; } = Guid.NewGuid();
    public ApplicationStatus? FromStatus { get; }
    public ApplicationStatus ToStatus { get; }
    public string ChangedBy { get; }
    public DateTime ChangedAt { get; }

    private ApplicationTimelineEntry()
    {
        // Для EF Core
    }

    public ApplicationTimelineEntry(
        ApplicationStatus? fromStatus,
        ApplicationStatus toStatus,
        string changedBy,
        DateTime changedAt)
    {
        FromStatus = fromStatus;
        ToStatus = toStatus;
        ChangedBy = changedBy;
        ChangedAt = changedAt;
    }
}