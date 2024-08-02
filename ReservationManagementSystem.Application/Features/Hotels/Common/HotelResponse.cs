namespace ReservationManagementSystem.Application.Features.Hotels.Common;

public sealed record HotelResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Name { get; set; }
}
