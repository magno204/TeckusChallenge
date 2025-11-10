using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TekusChallenge.Application.UseCases.Auth.Commands.Login;

namespace TekusChallenge.API.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        if (command == null)
        {
            return BadRequest("Credentials are required");
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return Unauthorized(result.Message);
        }

        return Ok(result);
    }
}
