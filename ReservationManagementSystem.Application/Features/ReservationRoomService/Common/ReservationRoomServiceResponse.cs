namespace ReservationManagementSystem.Application.Features.ReservationRoomService.Common;

public sealed record ReservationRoomServiceResponse
{
    public Guid ReservationRoomId { get; set; }
    public Guid HotelServiceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
