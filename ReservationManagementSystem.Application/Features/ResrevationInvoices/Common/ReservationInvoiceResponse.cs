using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Application.Features.ResrevationInvoices.Common;

public sealed record ReservationInvoiceResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ReservationId { get; set; }
    public decimal Amount { get; set; }
    public decimal Paid { get; set; }
    public decimal Due { get; set; }
    public Currencies Currency { get; set; }
}
