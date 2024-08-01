using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class ReservationRoom : BaseEntity
{
    public Guid ReservationId { get; set; }
    public Guid RateId { get; set; }
    public Guid RoomTypeId { get; set; }
    public Guid RoomId { get; set; } // specific Room assigned to a reservation
    public DateTime Checkin { get; set; }
    public DateTime Checkout { get; set; }
    public decimal Price { get; set; } // Sum of prices of all child ReservationRoomTimeline rows

    public Reservation? Reservation { get; set; }
    public RateRoomType? RateRoomType { get; set; }
    public Room? Room { get; set; }
    public ICollection<Guest>? Guests { get; set; }
    public ICollection<ReservationRoomTimeline>? ReservationRoomTimelines { get; set; } 
    public ICollection<ReservationRoomServices>? ReservationRoomServices { get; set; }
    public ICollection<ReservationRoomPayments>? ReservationRoomPayments { get; set; }
}
