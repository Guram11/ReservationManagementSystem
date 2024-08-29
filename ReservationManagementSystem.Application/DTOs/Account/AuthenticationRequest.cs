namespace ReservationManagementSystem.Application.DTOs.Account;

public sealed record AuthenticationRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public string? IpAddress { get; init; }
}
