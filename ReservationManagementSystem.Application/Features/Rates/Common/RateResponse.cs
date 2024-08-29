namespace ReservationManagementSystem.Application.Features.Rates.Common;

public sealed record RateResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public required string Name { get; init; }
    public Guid HotelId { get; init; }
}
