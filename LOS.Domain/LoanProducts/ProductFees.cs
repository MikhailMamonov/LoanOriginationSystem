using LOS.Domain.Common.Exceptions;

namespace LOS.Domain.LoanProducts;

public record ProductFees
{
    public decimal OriginationFeePercent { get; }
    public decimal EarlyRepaymentFeePercent { get; }
    public bool InsuranceRequired { get; }
    private ProductFees() { }

    public ProductFees(decimal originationFeePercent, decimal earlyRepaymentFeePercent, bool insuranceRequired)
    {
        if (originationFeePercent < 0 || originationFeePercent > 100)
            throw new BusinessRuleViolationException("InvalidOriginationFee", "Комиссия за выдачу должна быть от 0 до 100%.");

        if (earlyRepaymentFeePercent < 0 || earlyRepaymentFeePercent > 100)
            throw new BusinessRuleViolationException("InvalidEarlyRepaymentFee", "Комиссия за досрочное погашение должна быть от 0 до 100%.");

        OriginationFeePercent = originationFeePercent;
        EarlyRepaymentFeePercent = earlyRepaymentFeePercent;
        InsuranceRequired = insuranceRequired;
    }

    public decimal CalculateTotalFees(decimal loanAmount)
    {
        return loanAmount * (OriginationFeePercent / 100m);
    }
}