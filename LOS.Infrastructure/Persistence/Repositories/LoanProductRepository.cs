using LOS.Domain.LoanProducts;
using LOS.Domain.Common.Interfaces.Repositories;
using LOS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LOS.Infrastructure.Persistence.Repositories;

public class LoanProductRepository : ILoanProductRepository
{
    private readonly ApplicationDbContext _context;

    public LoanProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LoanProduct?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.LoanProducts.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<LoanProduct?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        return await _context.LoanProducts.FirstOrDefaultAsync(p => p.Code == code, ct);
    }

    public async Task<IEnumerable<LoanProduct>> GetAllActiveAsync(CancellationToken ct = default)
    {
        return await _context.LoanProducts.Where(p => p.IsActive).ToListAsync(ct);
    }

    // 👇 ДОБАВЛЯЕМ РЕАЛИЗАЦИЮ
    public async Task AddAsync(LoanProduct product, CancellationToken ct = default)
    {
        _context.LoanProducts.Add(product);
    }
}