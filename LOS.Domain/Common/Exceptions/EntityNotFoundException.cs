
namespace LOS.Domain.Common.Exceptions;

/// <summary>
/// Исключение, выбрасываемое, когда сущность не найдена в БД или в домене.
/// Например: клиент с таким ID не существует, продукт удален и т.д.
/// </summary>
public class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Имя типа сущности (например, "Client", "LoanApplication")
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Идентификатор сущности, которую не удалось найти
    /// </summary>
    public Guid EntityId { get; }

    /// <summary>
    /// Основной конструктор для использования в Application Service
    /// </summary>
    public EntityNotFoundException(string entityName, Guid entityId)
        : base($"Сущность '{entityName}' с ID {entityId} не найдена.")
    {
        EntityName = entityName;
        EntityId = entityId;
    }

    /// <summary>
    /// Конструктор для случаев, когда нужен пользовательский текст ошибки
    /// </summary>
    public EntityNotFoundException(string entityName, Guid entityId, string message)
        : base(message)
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}