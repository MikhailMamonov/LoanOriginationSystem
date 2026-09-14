using LOS.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOS.Infrastructure.Persistence.Configurations;

public class EmploymentConfiguration : IEntityTypeConfiguration<Employment>
{
    public void Configure(EntityTypeBuilder<Employment> builder)
    {
        builder.ToTable("employments");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.EmployerName).HasMaxLength(255).IsRequired();
        builder.Property(e => e.Position).HasMaxLength(150).IsRequired();
    }
}