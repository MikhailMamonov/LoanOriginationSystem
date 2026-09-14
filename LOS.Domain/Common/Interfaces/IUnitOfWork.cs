namespace LOS.Domain.Common.Interfaces;

/// <summary>
/// Интерфейс Unit of Work для управления транзакциями.
/// Абстракция над механизмом сохранения изменений в БД.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Сохраняет все изменения, сделанные в контексте, в базу данных.
    /// </summary>
    /// <param name="ct">Токен отмены</param>
    /// <returns>Количество измененных записей</returns>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}