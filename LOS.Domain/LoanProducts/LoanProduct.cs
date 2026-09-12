using LOS.Domain.Common.Exceptions;
using LOS.Domain.Common;
using LOS.Domain.Clients;
using System.Text.RegularExpressions;

namespace LOS.Domain.LoanProducts;

public class LoanProduct : BaseEntity
{
    /// <summary>
    /// Уникальный код продукта (например, "CONSUMER_2024")
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Название продукта
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Описание продукта
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Базовая годовая процентная ставка (%)
    /// </summary>
    public decimal BaseInterestRate { get; private set; }

    /// <summary>
    /// Минимальная сумма кредита
    /// </summary>
    public decimal MinAmount { get; private set; }

    /// <summary>
    /// Максимальная сумма кредита
    /// </summary>
    public decimal MaxAmount { get; private set; }

    /// <summary>
    /// Минимальный срок кредита (месяцев)
    /// </summary>
    public int MinTermMonths { get; private set; }

    /// <summary>
    /// Максимальный срок кредита (месяцев)
    /// </summary>
    public int MaxTermMonths { get; private set; }

    /// <summary>
    /// Активен ли продукт (доступен для новых заявок)
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Структура комиссий
    /// </summary>
    public ProductFees Fees { get; private set; }

    /// <summary>
    /// Критерии доступности продукта 
    /// </summary>
    public EligibilityCriteria Eligibility { get; private set; }

    public LoanProduct()
    {

    }

    public LoanProduct(string code,
        string name,
        string description,
        decimal baseInterestRate,
        decimal minAmount,
        decimal maxAmount,
        int minTermMonths,
        int maxTermMonths,
        ProductFees fees,
        EligibilityCriteria eligibility)
    {
        // Валидация обязательных параметров
        if (string.IsNullOrWhiteSpace(code))
            throw new BusinessRuleViolationException("ProductCodeEmpty", "Код продукта обязателен");

        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessRuleViolationException("ProductNameEmpty", "Название продукта обязательно");

        // Валидация инвариантов
        if (baseInterestRate <= 0)
            throw new BusinessRuleViolationException("InvalidBaseRate", "Базовая ставка должна быть больше 0");

        if (minAmount <= 0)
            throw new BusinessRuleViolationException("InvalidMinAmount", "Минимальная сумма должна быть больше 0");

        if (maxAmount <= minAmount)
            throw new BusinessRuleViolationException(
                "InvalidAmountRange",
                $"Максимальная сумма ({maxAmount}) должна быть больше минимальной ({minAmount})");

        if (minTermMonths <= 0)
            throw new BusinessRuleViolationException("InvalidMinTerm", "Минимальный срок должен быть больше 0");

        if (maxTermMonths <= minTermMonths)
            throw new BusinessRuleViolationException(
                "InvalidTermRange",
                $"Максимальный срок ({maxTermMonths}) должен быть больше минимального ({minTermMonths})");

        // Присваивание значений
        Code = code.Trim().ToUpper();
        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        BaseInterestRate = baseInterestRate;
        MinAmount = minAmount;
        MaxAmount = maxAmount;
        MinTermMonths = minTermMonths;
        MaxTermMonths = maxTermMonths;
        IsActive = true;
        Fees = fees ?? throw new ArgumentNullException(nameof(fees));
        Eligibility = eligibility ?? throw new ArgumentNullException(nameof(eligibility));

    }

    /// <summary>
    /// Проверяет, доступен ли продукт для клиента
    /// </summary>
    public bool IsClientEligible(Client client)
    {
        if (!IsActive) return false;
        if (client == null) return false;

        return Eligibility.IsSatisfiedBy(client);
    }

    /// <summary>
    /// Получает причины, по которым продукт недоступен для клиента
    /// </summary>
    public IEnumerable<string> GetEligibilityRejectionReasons(Client client)
    {
        if (!IsActive)
            yield return "Продукт неактивен";

        if (client == null)
        {
            yield return "Клиент не указан";
            yield break;
        }

        foreach (var reason in Eligibility.GetRejectionReasons(client))
            yield return reason;
    }

    /// <summary>
    /// Рассчитывает ежемесячный платеж (аннуитетный)
    /// </summary>
    /// <param name="amount">Сумма кредита</param>
    /// <param name="termMonths">Срок кредита (месяцев)</param>
    /// <param name="interestRate">Годовая процентная ставка (%)</param>
    /// <returns>Ежемесячный платеж</returns>
    public decimal CalculateMonthlyPayment(decimal amount, int termMonths, decimal? interestRate = null)
    {
        ValidateLoanParameters(amount, termMonths);

        decimal rate = interestRate ?? BaseInterestRate;
        decimal monthlyRate = rate / 12 / 100;

        if (monthlyRate == 0)
            return MathHelpers.Round(amount / termMonths);

        decimal power = MathHelpers.Pow(1 + monthlyRate, termMonths);

        decimal payment = amount * (monthlyRate * power) / (power - 1);

        return MathHelpers.Round(payment);
    }

    /// <summary>
    /// Деактивирует продукт (больше недоступен для новых заявок)
    /// </summary>
    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidEntityStateException(nameof(LoanProduct), Id, "Продукт уже деактивирован");

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidEntityStateException(nameof(LoanProduct), Id, "Продукт уже активен");

        IsActive = true;
    }

    public void UpdateInterestRate(decimal newRate)
    {
        if (newRate <= 0)
            throw new BusinessRuleViolationException("InvalidInterestRate", "Процентная ставка должна быть больше 0");

        BaseInterestRate = newRate;
    }

    private void ValidateLoanParameters(decimal amount, int termMonths)
    {
        if (amount < MinAmount || amount > MaxAmount)
            throw new BusinessRuleViolationException(
                "InvalidLoanAmount",
                $"Сумма кредита должна быть от {MinAmount} до {MaxAmount}. Получено: {amount}");

        if (termMonths < MinTermMonths || termMonths > MaxTermMonths)
            throw new BusinessRuleViolationException(
                "InvalidLoanTerm",
                $"Срок кредита должен быть от {MinTermMonths} до {MaxTermMonths} месяцев. Получено: {termMonths}");
    }
}