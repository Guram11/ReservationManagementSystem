namespace ReservationManagementSystem.Application.Features.RoomTypes.Common;

public sealed record RoomTypeResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid HotelId { get; set; }
    public required string Name { get; set; }
    public byte NumberOfRooms { get; set; }
    public bool IsActive { get; set; }
    public byte MinCapacity { get; set; }
    public byte MaxCapacity { get; set; }
}
