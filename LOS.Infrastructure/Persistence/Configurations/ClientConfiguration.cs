using LOS.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOS.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Email).HasMaxLength(255).IsRequired();
        builder.Property(e => e.Phone).HasMaxLength(20).IsRequired();
        builder.Property(e => e.PassportSeries).HasMaxLength(4).IsRequired();
        builder.Property(e => e.PassportNumber).HasMaxLength(6).IsRequired();

        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.Phone).IsUnique();
        builder.HasIndex(e => new { e.PassportSeries, e.PassportNumber }).IsUnique();

        builder.HasMany(e => e.Employments)
              .WithOne(e => e.Client)
              .HasForeignKey(e => e.ClientId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Incomes)
              .WithOne(e => e.Client)
              .HasForeignKey(e => e.ClientId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}