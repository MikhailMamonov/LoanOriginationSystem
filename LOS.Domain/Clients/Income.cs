using LOS.Domain.Common;

namespace LOS.Domain.Clients;

public class Income : BaseEntity
{
    public Guid ClientId { get; private set; }
    public Client Client { get; private set; }
    public decimal MonthlyAmount { get; private set; }
    public IncomeType Type { get; private set; }
    public bool IsConfirmed { get; private set; }

    private Income() { }

    public Income(Guid clientId, decimal monthlyAmount, IncomeType type)
    {
        ClientId = clientId;
        MonthlyAmount = monthlyAmount;
        Type = type;
        IsConfirmed = false;
    }

    public void Confirm()
    {
        IsConfirmed = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum IncomeType
{
    Salary,
    Business,
    Rental,
    Other
}