namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.CheckAvailibility;

public sealed record CheckAvailabilityResponse
{
    public Guid RoomTypeId { get; init; }
    public Guid RateId { get; init; }
    public required string RoomType { get; init; }
    public int AvailableRooms { get; init; }
    public decimal TotalPrice { get; init; }
}