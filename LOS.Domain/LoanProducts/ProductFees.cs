using LOS.Domain.Common.Exceptions;

namespace LOS.Domain.LoanProducts;

/// <summary>
/// Value Object, представляющий структуру комиссий кредитного продукта.
/// Иммутабелен, сравнивается по значению.
/// </summary>
public record ProductFees
{
    /// <summary>
    /// Комиссия за выдачу кредита (% от суммы)
    /// </summary>
    public decimal OriginationFeePercent { get; }

    /// <summary>
    /// Комиссия за досрочное погашение (% от остатка)
    /// </summary>
    public decimal EarlyRepaymentFeePercent { get; }

    /// <summary>
    /// Требуется ли страхование
    /// </summary>
    public bool InsuranceRequired { get; }

    private ProductFees() { } // Для EF Core

    public ProductFees(decimal originationFeePercent, decimal earlyRepaymentFeePercent, bool insuranceRequired)
    {
        if (originationFeePercent < 0 || originationFeePercent > 100)
            throw new BusinessRuleViolationException(
                "InvalidOriginationFee",
                $"Комиссия за выдачу должна быть от 0 до 100%. Получено: {originationFeePercent}");

        if (earlyRepaymentFeePercent < 0 || earlyRepaymentFeePercent > 100)
            throw new BusinessRuleViolationException(
                "InvalidEarlyRepaymentFee",
                $"Комиссия за досрочное погашение должна быть от 0 до 100%. Получено: {earlyRepaymentFeePercent}");

        OriginationFeePercent = originationFeePercent;
        EarlyRepaymentFeePercent = earlyRepaymentFeePercent;
        InsuranceRequired = insuranceRequired;
    }

    /// <summary>
    /// Рассчитывает сумму комиссий для заданной суммы кредита
    /// </summary>
    public decimal CalculateTotalFees(decimal loanAmount)
    {
        if (loanAmount < 0)
            throw new BusinessRuleViolationException("NegativeLoanAmount", "Сумма кредита не может быть отрицательной");

        return loanAmount * (OriginationFeePercent / 100m);
    }
}