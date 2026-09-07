using LOS.Domain.Common;
using LOS.Domain.Clients;
using Microsoft.EntityFrameworkCore;


namespace LOS.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Employment> Employments => Set<Employment>();
    public DbSet<Income> Incomes => Set<Income>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация Client
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("clients");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20).IsRequired();
            entity.Property(e => e.PassportSeries).HasMaxLength(4).IsRequired();
            entity.Property(e => e.PassportNumber).HasMaxLength(6).IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Phone).IsUnique();
            entity.HasIndex(e => new { e.PassportSeries, e.PassportNumber }).IsUnique();

            entity.HasMany(e => e.Employments)
                  .WithOne(e => e.Client)
                  .HasForeignKey(e => e.ClientId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Incomes)
                  .WithOne(e => e.Client)
                  .HasForeignKey(e => e.ClientId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Конфигурация Employment
        modelBuilder.Entity<Employment>(entity =>
        {
            entity.ToTable("employments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.EmployerName).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Position).HasMaxLength(150).IsRequired();
        });

        // Конфигурация Income
        modelBuilder.Entity<Income>(entity =>
        {
            entity.ToTable("incomes");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MonthlyAmount)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(e => e.Type)
                  .HasConversion<string>()
                  .HasMaxLength(50);
        });
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

