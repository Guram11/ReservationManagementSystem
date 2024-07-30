namespace ReservationManagementSystem.Infrastructure.Identity.Models.Email;

public class EmailRequest
{
    public required string To { get; set; }
    public required string Subject { get; set; }
    public required string Body { get; set; }
    public string? From { get; set; }
}
