namespace ReservationManagementSystem.Application.Features.AvailibilityTimeline.PushAvailability;

public sealed record AvailabilityResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime Date { get; init; }
    public Guid RoomTypeId { get; init; }
    public byte Available { get; init; }
}
