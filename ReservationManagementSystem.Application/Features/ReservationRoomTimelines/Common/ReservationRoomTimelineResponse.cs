namespace ReservationManagementSystem.Application.Features.ReservationRoomTimelines.Common;

public sealed record ReservationRoomTimelineResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid ReservationRoomId { get; init; }
    public DateTime Date { get; init; }
    public decimal Price { get; init; }
}
