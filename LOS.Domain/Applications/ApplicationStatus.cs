namespace LOS.Domain.LoanApplications;

public enum ApplicationStatus
{
    Draft,
    Submitted,
    ScoringInProgress,
    ScoringCompleted,
    Approved,
    Rejected,
    ManualReview,
    DocumentsCollection,
    Disbursed,
    Closed
}