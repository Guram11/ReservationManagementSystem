namespace ReservationManagementSystem.Application.Features.Rooms.Common;

public sealed record RoomResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid RoomTypeId { get; set; }
    public required string Number { get; set; }
    public byte Floor { get; set; }
    public string? Note { get; set; }
}
