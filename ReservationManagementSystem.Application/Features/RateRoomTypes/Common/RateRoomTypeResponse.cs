namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Common;

public sealed record RateRoomTypeResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid RateId { get; set; }
    public Guid RoomTypeId { get; set; }
}
