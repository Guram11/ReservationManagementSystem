using System.ComponentModel.DataAnnotations;

namespace ReservationManagementSystem.Application.DTOs.Account;

public class ResetPasswordRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Token { get; set; }
    [Required]
    [MinLength(6)]
    public required string Password { get; set; }

    [Required]
    [Compare("Password")]
    public required string ConfirmPassword { get; set; }
}
