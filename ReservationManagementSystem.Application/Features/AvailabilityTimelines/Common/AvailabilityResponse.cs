namespace ReservationManagementSystem.Application.Features.AvailabilityTimelines.Common;

public sealed record AvailabilityResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime Date { get; set; }
    public Guid RoomTypeId { get; set; }
    public byte Available { get; set; }
}
