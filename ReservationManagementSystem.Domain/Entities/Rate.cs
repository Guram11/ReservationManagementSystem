using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class Rate : BaseEntity
{
    public required string Name { get; set; }

    public Guid HotelId { get; set; }

    public Hotel? Hotel { get; set; }

    public ICollection<RateRoomType>? RateRoomTypes { get; set; }
}
