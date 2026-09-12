using LOS.Application.LoanProducts.Commands.CreateLoanProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoanProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateLoanProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = productId }, new { Id = productId });
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(Guid id)
    {
        return Ok(new { Id = id, Message = "Product retrieval endpoint" });
    }
}