using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.HotelServices.Common;

public sealed record HotelServiceResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid HotelId { get; set; }
    public HotelServiceTypes ServiceTypeId { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
}
