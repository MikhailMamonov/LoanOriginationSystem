namespace LOS.Application.Common.DTOs;

public record LoanApplicationDto(
    Guid Id,
    Guid ClientId,
    Guid ProductId,
    decimal RequestedAmount,
    int RequestedTermMonths,
    string Status,
    DateTime CreatedAt,
    IEnumerable<TimelineEntryDto> Timeline);

public record TimelineEntryDto(
    string? FromStatus,
    string ToStatus,
    string ChangedBy,
    DateTime ChangedAt);