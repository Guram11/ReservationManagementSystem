using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.Reservations.Common;

public sealed record ReservationResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid HotelId { get; set; }
    public DateTime Checkin { get; set; }
    public DateTime Checkout { get; set; }
    public Guid RoomTypeId { get; set; }
    public Guid RateId { get; set; }
    public int NumberOfRooms { get; set; }
    public Currencies Currency { get; set; }
    public ReservationStatus StatusId { get; set; }
}
