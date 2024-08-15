namespace ReservationManagementSystem.Application.Features.ReservationRooms.Common;

public sealed record ReservationRoomResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ReservationId { get; set; }
    public Guid RateId { get; set; }
    public Guid RoomTypeId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime Checkin { get; set; }
    public DateTime Checkout { get; set; }
    public decimal Price { get; set; }
}
