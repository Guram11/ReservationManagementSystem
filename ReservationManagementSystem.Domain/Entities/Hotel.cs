using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class Hotel : BaseEntity
{
    public required string Name { get; set; }

    public ICollection<RoomType> RoomTypes { get; set; }
    public ICollection<HotelServices> Services { get; set; }
}
