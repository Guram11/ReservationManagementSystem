using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class RateTimeline : BaseEntity
{
    public DateTime Date { get; set; }
    public Guid RateId { get; set; }
    public Guid RoomTypeId { get; set; }
    public decimal Price { get; set; }
    public RateRoomType? RateRoomType { get; set; }
}
