using System.ComponentModel.DataAnnotations;

namespace ReservationManagementSystem.Application.DTOs.Account;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
