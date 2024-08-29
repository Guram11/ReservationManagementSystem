namespace ReservationManagementSystem.Application.Features.RateRoomTypes.Common;

public sealed record RateRoomTypeResponse
{
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid RateId { get; init; }
    public Guid RoomTypeId { get; init; }
}
