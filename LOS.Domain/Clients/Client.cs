using LOS.Domain.Common;

namespace LOS.Domain.Clients;

public class Client : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? MiddleName { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string PassportSeries { get; private set; }
    public string PassportNumber { get; private set; }
    public string? Snils { get; private set; }
    public string? Inn { get; private set; }

    // Навигационные свойства
    public ICollection<Employment> Employments { get; private set; }
    public ICollection<Income> Incomes { get; private set; }

    private Client() { } // Для EF Core

    public Client(
        string firstName,
        string lastName,
        string email,
        string phone,
        DateTime birthDate,
        string passportSeries,
        string passportNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        BirthDate = birthDate;
        PassportSeries = passportSeries;
        PassportNumber = passportNumber;

        Employments = new List<Employment>();
        Incomes = new List<Income>();
    }

    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - BirthDate.Year;
        if (BirthDate.Date > today.AddYears(-age)) age--;
        return age;
    }

    public void UpdateContactInfo(string email, string phone)
    {
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }
}