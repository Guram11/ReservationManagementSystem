namespace ReservationManagementSystem.Application.Features.RoomTypes.Common;

public sealed record RoomTypeResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid HotelId { get; init; }
    public required string Name { get; init; }
    public byte NumberOfRooms { get; init; }
    public bool IsActive { get; init; }
    public byte MinCapacity { get; init; }
    public byte MaxCapacity { get; init; }
}
