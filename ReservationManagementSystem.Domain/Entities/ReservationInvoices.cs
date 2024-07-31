using ReservationManagementSystem.Domain.Common;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Domain.Entities;

public class ReservationInvoices : BaseEntity
{
    public Guid ReservationId { get; set; }
    public decimal Amount { get; set; }
    public decimal Paid { get; set; }
    public decimal Due { get; set; }
    public Currencies Currency { get; set; }

    public Reservation Reservation { get; set; }
}
