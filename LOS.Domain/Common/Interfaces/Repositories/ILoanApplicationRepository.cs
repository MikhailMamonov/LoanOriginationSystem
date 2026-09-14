using LOS.Domain.LoanApplications;

namespace LOS.Domain.Common.Interfaces.Repositories;

public interface ILoanApplicationRepository
{
    Task<LoanApplication?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<LoanApplication>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(LoanApplication application, CancellationToken ct = default);
}