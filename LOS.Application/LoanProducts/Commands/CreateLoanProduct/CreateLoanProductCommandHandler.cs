using LOS.Domain.Common.Interfaces;
using LOS.Domain.Common.Interfaces.Repositories;
using LOS.Domain.LoanProducts;
using MediatR;


namespace LOS.Application.LoanProducts.Commands.CreateLoanProduct;

public class CreateLoanProductCommandHandler : IRequestHandler<CreateLoanProductCommand, Guid>
{
    private readonly ILoanProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLoanProductCommandHandler(ILoanProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateLoanProductCommand request, CancellationToken ct)
    {
        // Создаем Value Objects
        var fees = new ProductFees(
            originationFeePercent: request.OriginationFeePercent,
            earlyRepaymentFeePercent: request.EarlyRepaymentFeePercent,
            insuranceRequired: request.InsuranceRequired);

        var eligibility = new EligibilityCriteria(
            minAge: request.MinAge,
            maxAge: request.MaxAge,
            minMonthlyIncome: request.MinMonthlyIncome,
            minWorkExperienceMonths: request.MinWorkExperienceMonths,
            citizenshipRequired: request.CitizenshipRequired,
            maxDebtToIncomeRatio: request.MaxDebtToIncomeRatio);

        // Создаем агрегат (инварианты проверятся внутри конструктора LoanProduct)
        var product = new LoanProduct(
            code: request.Code,
            name: request.Name,
            description: request.Description,
            baseInterestRate: request.BaseInterestRate,
            minAmount: request.MinAmount,
            maxAmount: request.MaxAmount,
            minTermMonths: request.MinTermMonths,
            maxTermMonths: request.MaxTermMonths,
            fees: fees,
            eligibility: eligibility);

        await _productRepository.AddAsync(product, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return product.Id;
    }
}