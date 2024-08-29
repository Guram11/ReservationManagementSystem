using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.Reservations.Common;

public sealed record ReservationResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid HotelId { get; init; }
    public DateTime Checkin { get; init; }
    public DateTime Checkout { get; init; }
    public Guid RoomTypeId { get; init; }
    public Guid RateId { get; init; }
    public int NumberOfRooms { get; init; }
    public Currencies Currency { get; init; }
    public ReservationStatus StatusId { get; init; }
}
