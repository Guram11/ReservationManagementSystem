using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class ReservationRoomServices : BaseEntity
{
    public Guid HotelServiceId { get; set; }
    public Guid ReservationRoomId { get; set; }

    public HotelServices HotelService { get; set; }
    public ReservationRoom ReservationRoom { get; set; }
}
