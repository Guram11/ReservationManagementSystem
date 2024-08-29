namespace ReservationManagementSystem.Application.Features.ReservationRooms.Common;

public sealed record ReservationRoomResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid ReservationId { get; init; }
    public Guid RateId { get; init; }
    public Guid RoomTypeId { get; init; }
    public Guid RoomId { get; init; }
    public DateTime Checkin { get; init; }
    public DateTime Checkout { get; init; }
    public decimal Price { get; init; }
}
