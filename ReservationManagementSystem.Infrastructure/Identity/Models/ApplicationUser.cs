using Microsoft.AspNetCore.Identity;
using ReservationManagementSystem.Application.DTOs.Account;

namespace ReservationManagementSystem.Infrastructure.Identity.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];
    public bool OwnsToken(string token)
    {
        return RefreshTokens?.Find(x => x.Token == token) != null;
    }
}
