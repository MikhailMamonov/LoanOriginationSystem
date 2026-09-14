using LOS.Domain.LoanApplications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOS.Infrastructure.Persistence.Configurations;

public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
{
    public void Configure(EntityTypeBuilder<LoanApplication> builder)
    {
        builder.ToTable("loan_applications");
        builder.HasKey(x => x.Id);

        // Простые поля
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.RequestedAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.RequestedTermMonths).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().IsRequired();

        // Таблица для ApplicationTimelineEntry в той же таблице
        builder.OwnsMany(x => x.Timeline, timelineBuilder =>
        {
            timelineBuilder.ToTable("application_timeline");
            timelineBuilder.WithOwner().HasForeignKey("ApplicationId");

            timelineBuilder.Property(e => e.FromStatus).HasConversion<string>();
            timelineBuilder.Property(e => e.ToStatus).HasConversion<string>();
            timelineBuilder.Property(e => e.ChangedBy).HasMaxLength(200);
        });
    }
}