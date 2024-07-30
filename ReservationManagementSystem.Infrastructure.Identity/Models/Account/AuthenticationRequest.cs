namespace ReservationManagementSystem.Infrastructure.Identity.Models.Account;

public class AuthenticationRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
