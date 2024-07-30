using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.DTOs.Account;
using ReservationManagementSystem.Application.Interfaces.Services;

namespace ReservationManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAccountService _accountService;
    public AuthController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
    {
        return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        var origin = Request.Headers.Origin;
        return Ok(await _accountService.RegisterAsync(request, origin));
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
    {
        return Ok(await _accountService.ConfirmEmailAsync(userId, code));
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
    {
        await _accountService.ForgotPassword(model, Request.Headers.Origin);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
    {
        return Ok(await _accountService.ResetPassword(model));
    }

    private string GenerateIPAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Request.Headers["X-Forwarded-For"];
        else
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
}
