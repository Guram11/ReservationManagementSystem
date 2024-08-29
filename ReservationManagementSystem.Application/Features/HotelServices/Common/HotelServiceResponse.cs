using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.HotelServices.Common;

public sealed record HotelServiceResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid HotelId { get; init; }
    public HotelServiceTypes ServiceTypeId { get; init; }
    public required string Description { get; init; }
    public decimal Price { get; init; }
}
