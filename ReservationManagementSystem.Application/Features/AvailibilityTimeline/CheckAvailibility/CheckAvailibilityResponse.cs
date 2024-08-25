namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;

public sealed record CheckAvailabilityResponse
{
    public Guid RoomTypeId  { get; set; }
    public Guid RateId  { get; set; }
    public required string RoomType { get; set; }
    public int AvailableRooms { get; set; }
    public decimal TotalPrice { get; set; }
}
