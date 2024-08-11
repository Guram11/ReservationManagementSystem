namespace ReservationManagementSystem.Application.Features.RateTimelines.Common;

public sealed record RateTimelineResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime Date { get; set; }
    public Guid RateId { get; set; }
    public Guid RoomTypeId { get; set; }
    public decimal Price { get; set; }
}
