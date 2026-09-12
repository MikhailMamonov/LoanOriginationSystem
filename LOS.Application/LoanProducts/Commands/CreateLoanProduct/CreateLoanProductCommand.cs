using MediatR;

namespace LOS.Application.LoanProducts.Commands.CreateLoanProduct;

public record CreateLoanProductCommand(
    string Code,
    string Name,
    string Description,
    decimal BaseInterestRate,
    decimal MinAmount,
    decimal MaxAmount,
    int MinTermMonths,
    int MaxTermMonths,
    // Fees
    decimal OriginationFeePercent,
    decimal EarlyRepaymentFeePercent,
    bool InsuranceRequired,
    // Eligibility
    int MinAge,
    int MaxAge,
    decimal MinMonthlyIncome,
    int MinWorkExperienceMonths,
    string CitizenshipRequired,
    decimal MaxDebtToIncomeRatio) : IRequest<Guid>;

