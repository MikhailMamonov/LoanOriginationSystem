using LOS.Domain.Common.Exceptions;
using LOS.Domain.Common;
using System.Text.RegularExpressions;

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
        if (string.IsNullOrWhiteSpace(firstName))
            throw new BusinessRuleViolationException("ClientFirstNameEmpty", "Имя не может быть пустым.");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new BusinessRuleViolationException("ClientLastNameEmpty", "Фамилия не может быть пустой.");

        if (!IsValidEmail(email))
            throw new BusinessRuleViolationException("ClientEmailInvalid", "Некорректный формат Email.");

        if (!IsValidPhone(phone))
            throw new BusinessRuleViolationException("ClientPhoneInvalid", "Некорректный формат телефона. Ожидается +7XXXXXXXXXX или 8XXXXXXXXXX.");

        int age = CalculateAge(birthDate);
        if (age < 14 || age > 100)
            throw new BusinessRuleViolationException("ClientAgeInvalid", $"Возраст клиента должен быть от 14 до 100 лет. Текущий возраст: {age}.");

        if (!Regex.IsMatch(passportSeries, @"^\d{4}$"))
            throw new BusinessRuleViolationException("ClientPassportSeriesInvalid", "Серия паспорта должна состоять из 4 цифр.");
        if (!Regex.IsMatch(passportNumber, @"^\d{6}$"))
            throw new BusinessRuleViolationException("ClientPassportNumberInvalid", "Номер паспорта должен состоять из 6 цифр.");

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

    public void AddIncome(Income income)
    {
        if (income == null)
        {
            throw new ArgumentException(nameof(income));
        }
        Incomes.Add(income);
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddEmployment(Employment employment)
    {
        if (employment == null)
        {
            throw new ArgumentException(nameof(employment));
        }
        Employments.Add(employment);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateContactInfo(string email, string phone)
    {
        if (!IsValidEmail(email))
            throw new BusinessRuleViolationException("InvalidEmail", "Некорректный формат Email.");

        if (!IsValidPhone(phone))
            throw new ArgumentException("InvalidPhone", "Некорректный формат телефона.");

        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
            throw new InvalidOperationException("Должен быть указан хотя бы один способ связи.");

        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }

    private int CalculateAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
    private bool IsValidPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        string pattern = @"^(\+7|8)[0-9]{10}$";
        return Regex.IsMatch(phone, pattern);
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }
}