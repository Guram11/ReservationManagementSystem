namespace ReservationManagementSystem.Application.Features.Hotels.Common;

public sealed record HotelResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public required string Name { get; init; }
}
