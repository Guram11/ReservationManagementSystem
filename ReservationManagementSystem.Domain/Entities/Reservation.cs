using ReservationManagementSystem.Domain.Common;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Domain.Entities;

public class Reservation : BaseEntity
{
    public Guid HotelId { get; set; }
    public DateTime Checkin { get; set; }
    public DateTime Checkout { get; set; }
    public Guid RoomTypeId { get; set; }
    public Guid RateId { get; set; }
    public int NumberOfRooms { get; set; }
    public Currencies Currency { get; set; }
    public ReservationStatus StatusId { get; set; }
    public Hotel? Hotel { get; set; }
    public ICollection<ReservationRoom>? ReservationRooms { get; set; }
    public ICollection<ReservationInvoices>? Invoices { get; set; }
}
