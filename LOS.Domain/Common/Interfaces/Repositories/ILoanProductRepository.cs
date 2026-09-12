using LOS.Domain.LoanProducts;

namespace LOS.Domain.Common.Interfaces.Repositories;

public interface ILoanProductRepository
{
    Task<LoanProduct?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<LoanProduct?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IEnumerable<LoanProduct>> GetAllActiveAsync(CancellationToken ct = default);
    Task AddAsync(LoanProduct product, CancellationToken ct = default);
}