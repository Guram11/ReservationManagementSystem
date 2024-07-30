using System.ComponentModel.DataAnnotations;

namespace ReservationManagementSystem.Application.DTOs.Account;

public class RegisterRequest
{
    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [MinLength(6)]
    public required string UserName { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }

    [Required]
    [Compare("Password")]
    public required string ConfirmPassword { get; set; }
}
