// using LOS.Domain.Clients;
// using LOS.Domain.LoanProducts;
// using LOS.Domain.Common.Exceptions;

// namespace LOS.Application.LoanProducts.Queries;

// public record GetAvailableProductsQuery(Guid ClientId) : IRequest<IEnumerable<AvailableProductDto>>;

// public record AvailableProductDto(
//     Guid ProductId,
//     string Code,
//     string Name,
//     string Description,
//     decimal BaseInterestRate,
//     decimal MinAmount,
//     decimal MaxAmount,
//     int MinTermMonths,
//     int MaxTermMonths,
//     bool InsuranceRequired);


// public class GetAvailableProductsQueryHandler 
//     : IRequestHandler<GetAvailableProductsQuery, IEnumerable<AvailableProductDto>>
// {
//     private readonly ILoanProductRepository _productRepository;
//     private readonly IClientRepository _clientRepository;

//     public GetAvailableProductsQueryHandler(
//         ILoanProductRepository productRepository,
//         IClientRepository clientRepository)
//     {
//         _productRepository = productRepository;
//         _clientRepository = clientRepository;
//     }

//     public async Task<IEnumerable<AvailableProductDto>> Handle(
//         GetAvailableProductsQuery request, 
//         CancellationToken cancellationToken)
//     {
//         var client = await _clientRepository.GetByIdAsync(request.ClientId, cancellationToken);
//         if (client == null)
//             throw new EntityNotFoundException(nameof(Client), request.ClientId);

//         var allProducts = await _productRepository.GetAllActiveAsync(cancellationToken);

//         var availableProducts = allProducts
//             .Where(p => p.IsClientEligible(client))
//             .Select(p => new AvailableProductDto(
//                 p.Id,
//                 p.Code,
//                 p.Name,
//                 p.Description,
//                 p.BaseInterestRate,
//                 p.MinAmount,
//                 p.MaxAmount,
//                 p.MinTermMonths,
//                 p.MaxTermMonths,
//                 p.Fees.InsuranceRequired))
//             .ToList();

//         return availableProducts;
//     }
// }