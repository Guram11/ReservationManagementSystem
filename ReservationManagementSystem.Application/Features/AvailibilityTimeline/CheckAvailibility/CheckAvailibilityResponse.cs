namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;

public sealed record CheckAvailabilityResponse
{
    public required string RoomType { get; set; }
    public int AvailableRooms { get; set; }
    public decimal TotalPrice { get; set; }
}
