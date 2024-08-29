using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;

public sealed record ReservationRoomPaymentsResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid ReservationRoomId { get; init; }
    public decimal Amount { get; init; }
    public required string Description { get; init; }
    public Currencies Currency { get; init; }
}
