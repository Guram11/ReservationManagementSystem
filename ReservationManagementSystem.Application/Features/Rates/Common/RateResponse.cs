namespace ReservationManagementSystem.Application.Features.Rates.Common;

public sealed record RateResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
    public Guid HotelId { get; set; }
}
