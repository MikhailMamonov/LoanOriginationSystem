namespace LOS.Domain.Common.Exceptions;

public class BusinessRuleViolationException : DomainException
{
    public string RuleName { get; set; }

    public BusinessRuleViolationException(string ruleName, string message)
        : base(message, $"RULE_VIOLATION_{ruleName.ToUpperInvariant()}")
    {
        RuleName = ruleName;
    }
}