using LOS.Domain.LoanProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOS.Infrastructure.Persistence.Configurations;

public class LoanProductConfiguration : IEntityTypeConfiguration<LoanProduct>
{
    public void Configure(EntityTypeBuilder<LoanProduct> builder)
    {
        builder.ToTable("loan_products");
        builder.HasKey(x => x.Id);

        // Основные поля
        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2000);

        builder.Property(x => x.BaseInterestRate)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(x => x.MinAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.MaxAmount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        // Уникальный индекс на код продукта
        builder.HasIndex(x => x.Code).IsUnique();

        // Value Object: Fees
        builder.OwnsOne(x => x.Fees, feesBuilder =>
        {
            feesBuilder.Property(f => f.OriginationFeePercent)
                .HasColumnName("fee_origination_percent")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            feesBuilder.Property(f => f.EarlyRepaymentFeePercent)
                .HasColumnName("fee_early_repayment_percent")
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            feesBuilder.Property(f => f.InsuranceRequired)
                .HasColumnName("fee_insurance_required")
                .HasDefaultValue(false);
        });

        // Value Object: EligibilityCriteria
        builder.OwnsOne(x => x.Eligibility, eligBuilder =>
        {
            eligBuilder.Property(e => e.MinAge)
                .HasColumnName("eligibility_min_age")
                .IsRequired();

            eligBuilder.Property(e => e.MaxAge)
                .HasColumnName("eligibility_max_age")
                .IsRequired();

            eligBuilder.Property(e => e.MinMonthlyIncome)
                .HasColumnName("eligibility_min_income")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            eligBuilder.Property(e => e.MinWorkExperienceMonths)
                .HasColumnName("eligibility_min_work_experience_months")
                .IsRequired();

            eligBuilder.Property(e => e.CitizenshipRequired)
                .HasColumnName("eligibility_citizenship_required")
                .HasMaxLength(10)
                .HasDefaultValue("RU");

            eligBuilder.Property(e => e.MaxDebtToIncomeRatio)
                .HasColumnName("eligibility_max_debt_to_income_ratio")
                .HasColumnType("decimal(3,2)")
                .IsRequired();
        });
    }
}

