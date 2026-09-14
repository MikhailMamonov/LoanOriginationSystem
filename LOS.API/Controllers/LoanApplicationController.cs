using LOS.Application.LoanApplications.Commands.CreateApplication;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using LOS.Domain.Common.Exceptions;

namespace LOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoanApplicationController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoanApplicationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Создает новую заявку на кредит
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationCommand command)
    {
        try
        {
            Guid applicationId = await _mediator.Send(command);

            return CreatedAtAction(
                nameof(GetApplication),
                new { id = applicationId },
                new { Id = applicationId });
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { ex.ErrorCode, ex.Message });
        }
        catch (BusinessRuleViolationException ex)
        {
            return UnprocessableEntity(new { ex.ErrorCode, ex.Message });
        }
    }

    /// <summary>
    /// Получает информацию о заявке по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetApplication(Guid id)
    {
        try
        {
            return Ok(new { Id = id, Message = "Product retrieval endpoint" });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    // /// <summary>
    // /// Подает заявку на рассмотрение (переводит из Draft в Submitted)
    // /// </summary>
    // [HttpPost("{id}/submit")]
    // public async Task<IActionResult> SubmitApplication(Guid id)
    // {
    //     try
    //     {
    //         var command = new SubmitApplicationCommand(id);
    //         var status = await _mediator.Send(command);
    //         return Ok(new { Status = status });
    //     }
    //     catch (EntityNotFoundException)
    //     {
    //         return NotFound();
    //     }
    //     catch (InvalidEntityStateException ex)
    //     {
    //         return Conflict(new 
    //         { 
    //             ex.ErrorCode, 
    //             ex.EntityId, 
    //             ex.CurrentState, 
    //             ex.Message 
    //         });
    //     }
    // }

    // /// <summary>
    // /// Запускает процесс скоринга для заявки
    // /// </summary>
    // [HttpPost("{id}/start-scoring")]
    // public async Task<IActionResult> StartScoring(Guid id)
    // {
    //     try
    //     {
    //         var command = new StartScoringCommand(id);
    //         var status = await _mediator.Send(command);
    //         return Ok(new { Status = status });
    //     }
    //     catch (EntityNotFoundException)
    //     {
    //         return NotFound();
    //     }
    //     catch (InvalidEntityStateException ex)
    //     {
    //         return Conflict(new 
    //         { 
    //             ex.ErrorCode, 
    //             ex.EntityId, 
    //             ex.CurrentState, 
    //             ex.Message 
    //         });
    //     }
    // }
}

// DTO для запроса создания (остается в API слое или выносится в Application.Contracts)
public record CreateApplicationRequest(
    Guid ClientId,
    Guid ProductId,
    decimal RequestedAmount,
    int RequestedTermMonths);