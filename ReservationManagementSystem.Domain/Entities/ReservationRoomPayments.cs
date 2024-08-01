using ReservationManagementSystem.Domain.Common;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Domain.Entities;

public class ReservationRoomPayments : BaseEntity
{
    public Guid ReservationRoomId { get; set; }
    public decimal Amount { get; set; }
    public required string Description { get; set; }
    public Currencies Currency { get; set; }

    public ReservationRoom? ReservationRoom { get; set; }
}
