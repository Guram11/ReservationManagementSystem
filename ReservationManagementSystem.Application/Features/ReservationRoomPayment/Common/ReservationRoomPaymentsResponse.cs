using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.ReservationRoomPayment.Common;

public sealed record ReservationRoomPaymentsResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ReservationRoomId { get; set; }
    public decimal Amount { get; set; }
    public required string Description { get; set; }
    public Currencies Currency { get; set; }
}
