namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Common;

public sealed record RateRoomTypeResponse
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid RateId { get; set; }
    public Guid RoomTypeId { get; set; }
}
