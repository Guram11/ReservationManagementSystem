namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Common;

public sealed record ReservationRoomServiceResponse
{
    public Guid ReservationRoomId { get; init; }
    public Guid HotelServiceId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
