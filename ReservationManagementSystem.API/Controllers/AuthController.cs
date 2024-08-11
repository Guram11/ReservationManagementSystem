using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Features.Users.Commands.AuthenticateUser;
using ReservationManagementSystem.Application.Features.Users.Commands.ConfirmEmail;
using ReservationManagementSystem.Application.Features.Users.Commands.ForgotPassword;
using ReservationManagementSystem.Application.Features.Users.Commands.RegisterUser;
using ReservationManagementSystem.Application.Features.Users.Commands.ResetPassword;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("authenticate")]
    public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] AuthenticateUserRequest request)
    {
        var origin = Request.Headers["Origin"].FirstOrDefault();
        request.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return Unauthorized(result.Error);
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register([FromBody] CreateUserRequest request)
    {
        var origin = Request.Headers["Origin"].FirstOrDefault();
        request.Origin = origin;
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Error);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
    {
        var result = await _mediator.Send(new ConfirmEmailRequest { UserId = userId, Code = code });
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Error);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<string>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var origin = Request.Headers["Origin"].FirstOrDefault();
        request.Origin = origin;
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Error);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<string>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Error);
    }
}
