using LOS.Domain.Clients;

namespace LOS.Domain.Common.Interfaces.Repositories;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Client client, CancellationToken ct = default);
}