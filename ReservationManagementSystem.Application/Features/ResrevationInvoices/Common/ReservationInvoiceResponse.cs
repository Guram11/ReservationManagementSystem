using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;

public sealed record ReservationInvoiceResponse
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public Guid ReservationId { get; init; }
    public decimal Amount { get; init; }
    public decimal Paid { get; init; }
    public decimal Due { get; init; }
    public Currencies Currency { get; init; }
}
