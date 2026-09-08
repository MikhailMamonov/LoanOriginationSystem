
using LOS.Domain.LoanProducts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LOS.Infrastructure.Persistence.Configurations;

public class LoanProductsConfiguration : IEntityTypeConfiguration<LoanProduct>
{
    public void Configure(EntityTypeBuilder<LoanProduct> builder)
    {
        builder.ToTable("loan_products");
        builder.HasKey(x => x.Id);

        builder.Property(f => f.Code).HasColumnName("code");
        builder.Property(f => f.Name).HasColumnName("name");
        builder.Property(f => f.Description).HasColumnName("description");
        builder.Property(f => f.BaseInterestRate).HasColumnName("base_interest_rate");
        builder.Property(f => f.IsActive).HasColumnName("is_active");

        builder.OwnsOne(p => p.Fees, feeBuilder =>
        {
            feeBuilder.Property(f => f.OriginationFeePercent).HasColumnName("fee_origination_percent");
            feeBuilder.Property(f => f.EarlyRepaymentFeePercent).HasColumnName("fee_early_repayment_percent");
            feeBuilder.Property(f => f.InsuranceRequired).HasColumnName("fee_insurance_required");
        });

        builder.OwnsOne(p => p.Eligibility, eligBuilder =>
        {
            eligBuilder.Property(e => e.MinAge).HasColumnName("eligibility_min_age");
            eligBuilder.Property(e => e.MaxAge).HasColumnName("eligibility_max_age");
            eligBuilder.Property(e => e.MinMonthlyIncome).HasColumnName("eligibility_min_income");
            eligBuilder.Property(e => e.MinWorkExperienceMonths).HasColumnName("eligibility_min_work_experience_months");
            eligBuilder.Property(e => e.CitizenshipRequired).HasColumnName("eligibility_citizenship_required");
            eligBuilder.Property(e => e.MaxDebtToIncomeRatio).HasColumnName("eligibility_max_debt_to_income_ratio");
        });
    }
}

