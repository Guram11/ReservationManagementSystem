namespace ReservationManagementSystem.Application.Features.Rooms.Common;

public sealed record RoomResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid RoomTypeId { get; init; }
    public required string Number { get; init; }
    public byte Floor { get; init; }
    public string? Note { get; init; }
}
