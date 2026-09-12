using LOS.Application.Clients.Commands.CreateClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientCommand command)
    {
        var clientId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetClient), new { id = clientId }, new { Id = clientId });
    }

    [HttpGet("{id}")]
    public IActionResult GetClient(Guid id)
    {
        // Для простоты тестирования пока вернем заглушку или реализуйте Query аналогично Command
        return Ok(new { Id = id, Message = "Client retrieval endpoint (implement Query if needed)" });
    }
}