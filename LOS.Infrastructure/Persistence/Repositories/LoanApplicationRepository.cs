using LOS.Domain.LoanApplications;
using LOS.Domain.Common.Interfaces.Repositories;
using LOS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LOS.Infrastructure.Persistence.Repositories;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public LoanApplicationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LoanApplication?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.LoanApplications
            .Include(a => a.Timeline)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<IEnumerable<LoanApplication>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.LoanApplications
            .Include(a => a.Timeline)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task AddAsync(LoanApplication application, CancellationToken ct = default)
    {
        await _context.LoanApplications.AddAsync(application, ct);
    }

}