namespace ReservationManagementSystem.Application.Features.ReservationRoomTimelines.Common;

public sealed record ReservationRoomTimelineResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ReservationRoomId { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
}
