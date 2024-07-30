using System.ComponentModel.DataAnnotations;

namespace ReservationManagementSystem.Infrastructure.Identity.Models.Account;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
