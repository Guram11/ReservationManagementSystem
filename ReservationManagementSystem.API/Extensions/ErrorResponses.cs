namespace ReservationManagementSystem.API.Extensions;

public class CustomErrorResponse
{
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
}
