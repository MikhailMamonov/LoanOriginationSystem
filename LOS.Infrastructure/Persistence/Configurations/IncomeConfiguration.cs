using LOS.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOS.Infrastructure.Persistence.Configurations;

public class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder.ToTable("incomes");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.MonthlyAmount)
              .HasColumnType("decimal(18,2)")
              .IsRequired();

        builder.Property(e => e.Type)
              .HasConversion<string>()
              .HasMaxLength(50);
    }
}
