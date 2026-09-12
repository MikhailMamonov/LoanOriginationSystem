using LOS.Domain.Common.Exceptions;
using LOS.Domain.Clients;

namespace LOS.Domain.LoanProducts;

public record EligibilityCriteria
{
    /// <summary>
    /// Минимально допустимый возраст клиента (в годах) на момент подачи заявки.
    /// </summary>
    public int MinAge { get; }

    /// <summary>
    /// Максимально допустимый возраст клиента (в годах) на момент подачи заявки.
    /// </summary>
    public int MaxAge { get; }

    /// <summary>
    /// Минимальный подтвержденный ежемесячный доход клиента (в рублях), 
    /// необходимый для обслуживания данного кредита.
    /// </summary>
    public decimal MinMonthlyIncome { get; }

    /// <summary>
    /// Минимальный непрерывный стаж работы на текущем месте (в месяцах), 
    /// </ </summary>
    public int MinWorkExperienceMonths { get; }

    /// <summary>
    /// Код требуемого гражданства (например, "RU"). 
    /// </summary>
    public string CitizenshipRequired { get; }

    /// <summary>
    /// Максимально допустимый коэффициент долговой нагрузки (ПДН).
    /// </summary>
    public decimal MaxDebtToIncomeRatio { get; }

    private EligibilityCriteria() { }

    public EligibilityCriteria(int minAge, int maxAge, decimal minMonthlyIncome,
                               int minWorkExperienceMonths, string citizenshipRequired, decimal maxDebtToIncomeRatio)
    {
        if (minAge < 18)
            throw new BusinessRuleViolationException("InvalidMinAge", "Минимальный возраст не может быть меньше 18 лет");

        if (minAge >= maxAge)
            throw new BusinessRuleViolationException(
                "InvalidAgeRange",
                $"Минимальный возраст ({minAge}) должен быть меньше максимального ({maxAge})");

        if (maxAge > 100)
            throw new BusinessRuleViolationException("InvalidMaxAge", "Максимальный возраст не может превышать 100 лет");

        if (minMonthlyIncome < 0)
            throw new BusinessRuleViolationException("NegativeMinIncome", "Минимальный доход не может быть отрицательным");

        if (minWorkExperienceMonths < 0)
            throw new BusinessRuleViolationException("NegativeWorkExperience", "Требуемый стаж не может быть отрицательным");

        if (maxDebtToIncomeRatio <= 0 || maxDebtToIncomeRatio > 1.0m)
            throw new BusinessRuleViolationException(
                "InvalidDebtToIncomeRatio",
                $"Коэффициент долговой нагрузки должен быть от 0 до 1.0. Получено: {maxDebtToIncomeRatio}");


        MinAge = minAge;
        MaxAge = maxAge;
        MinMonthlyIncome = minMonthlyIncome;
        MinWorkExperienceMonths = minWorkExperienceMonths;
        CitizenshipRequired = citizenshipRequired;
        MaxDebtToIncomeRatio = maxDebtToIncomeRatio;
    }

    /// <summary>
    /// Specification Pattern: проверяет, удовлетворяет ли клиент критериям продукта
    /// </summary>
    public bool IsSatisfiedBy(Client client)
    {
        if (client == null) return false;

        // Проверка возраста
        int age = client.GetAge();
        if (age < MinAge || age > MaxAge)
            return false;

        // Проверка подтвержденного дохода
        decimal totalConfirmedIncome = client.Incomes
            .Where(i => i.IsConfirmed)
            .Sum(i => i.MonthlyAmount);

        if (totalConfirmedIncome < MinMonthlyIncome)
            return false;

        // Проверка стажа работы (берем текущее место работы)
        var currentEmployment = client.Employments
            .FirstOrDefault(e => e.IsCurrent);

        if (currentEmployment == null && MinWorkExperienceMonths > 0)
            return false;

        if (currentEmployment != null)
        {
            var workExperienceMonths = CalculateWorkExperienceMonths(currentEmployment.StartDate);
            if (workExperienceMonths < MinWorkExperienceMonths)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Получить причины, по которым клиент не подходит
    /// </summary>
    public IEnumerable<string> GetRejectionReasons(Client client)
    {
        var reasons = new List<string>();

        int age = client.GetAge();
        if (age < MinAge)
            reasons.Add($"Возраст клиента ({age}) меньше минимального ({MinAge})");
        if (age > MaxAge)
            reasons.Add($"Возраст клиента ({age}) больше максимального ({MaxAge})");

        decimal totalConfirmedIncome = client.Incomes
            .Where(i => i.IsConfirmed)
            .Sum(i => i.MonthlyAmount);

        if (totalConfirmedIncome < MinMonthlyIncome)
            reasons.Add($"Подтвержденный доход ({totalConfirmedIncome:F2}) меньше требуемого ({MinMonthlyIncome:F2})");

        var currentEmployment = client.Employments.FirstOrDefault(e => e.IsCurrent);
        if (currentEmployment != null)
        {
            var workExperienceMonths = CalculateWorkExperienceMonths(currentEmployment.StartDate);
            if (workExperienceMonths < MinWorkExperienceMonths)
                reasons.Add($"Стаж работы ({workExperienceMonths} мес.) меньше требуемого ({MinWorkExperienceMonths} мес.)");
        }
        else if (MinWorkExperienceMonths > 0)
        {
            reasons.Add("Отсутствует текущее место работы");
        }

        return reasons;
    }

    private static int CalculateWorkExperienceMonths(DateTime startDate)
    {
        var today = DateTime.Today;
        var months = (today.Year - startDate.Year) * 12 + today.Month - startDate.Month;
        return Math.Max(0, months);
    }
}