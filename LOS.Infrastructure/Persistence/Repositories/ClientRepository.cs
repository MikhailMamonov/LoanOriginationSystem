using LOS.Domain.Common.Interfaces.Repositories;
using LOS.Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace LOS.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Clients
            .Include(c => c.Employments)
            .Include(c => c.Incomes)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task AddAsync(Client client, CancellationToken ct = default)
    {
        await _context.Clients.AddAsync(client, ct);
    }
}