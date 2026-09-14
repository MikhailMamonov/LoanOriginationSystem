using LOS.Domain.Common;
using LOS.Domain.Clients;
using LOS.Domain.LoanProducts;
using LOS.Domain.LoanApplications;
using System.Reflection;
using Microsoft.EntityFrameworkCore;


namespace LOS.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Employment> Employments => Set<Employment>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<LoanProduct> LoanProducts => Set<LoanProduct>();
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

