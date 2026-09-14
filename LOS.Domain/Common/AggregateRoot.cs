using System;

namespace LOS.Domain.Common;

/// <summary>
/// Базовая сущность для агрегатов.
/// Отличается от BaseEntity тем, что явно обозначает корень агрегата.
/// </summary>
public abstract class AggregateRoot : BaseEntity
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(Guid id, DateTime createdAt)
        : base(id, createdAt)
    {
    }

    // Здесь можно добавить общие методы для всех агрегатов
    // Например: методы для работы с Domain Events
}