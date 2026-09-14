namespace LOS.Domain.Common.Exceptions;

/// <summary>
/// Исключение, возникающее при попытке выполнить операцию над сущностью, 
/// которая находится в недопустимом для этого состоянии.
/// </summary>
public class InvalidEntityStateException : DomainException
{
    /// <summary>
    /// Имя типа сущности (например, "LoanApplication", "Client")
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Идентификатор конкретной сущности (для логирования и отладки)
    /// </summary>
    public Guid EntityId { get; }

    /// <summary>
    /// Текущее состояние сущности
    /// </summary>
    public string? CurrentState { get; }

    /// <summary>
    /// Основной конструктор
    /// </summary>
    /// <param name="entityName">Имя сущности (обычно nameof(Entity))</param>
    /// <param name="entityId">ID сущности</param>
    /// <param name="message">Человекочитаемое описание ошибки</param>
    /// <param name="currentState">Текущее состояние (например, "Approved")</param>
    public InvalidEntityStateException(
        string entityName,
        Guid entityId,
        string message,
        string? currentState = null)
        : base(message, GenerateErrorCode(entityName))
    {
        EntityName = entityName;
        EntityId = entityId;
        CurrentState = currentState;
    }

    /// <summary>
    /// Генерирует машиночитаемый код ошибки для API
    /// Пример: INVALID_STATE_LOAN_APPLICATION
    /// </summary>
    private static string GenerateErrorCode(string entityName)
    {
        return $"INVALID_STATE_{entityName.ToUpperInvariant()}";
    }
}