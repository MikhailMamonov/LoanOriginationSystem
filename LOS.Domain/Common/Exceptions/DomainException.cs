
namespace LOS.Domain.Common.Exceptions;

public class DomainException : Exception
{
    public string ErrorCode { get; set; }

    public DomainException(string message) : base(message)
    {
        ErrorCode = "DOMAIN_ERROR";
    }

    public DomainException(string message, string errorCode)
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = "DOMAIN_ERROR";
    }
}