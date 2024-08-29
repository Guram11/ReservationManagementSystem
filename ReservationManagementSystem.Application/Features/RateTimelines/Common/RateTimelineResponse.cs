namespace ReservationManagementSystem.Application.Features.RateTimelines.Common;

public sealed record RateTimelineResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime Date { get; init; }
    public Guid RateId { get; init; }
    public Guid RoomTypeId { get; init; }
    public decimal Price { get; init; }
}
