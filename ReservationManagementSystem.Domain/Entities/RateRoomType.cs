using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class RateRoomType : BaseEntity
{
    public Guid RateId { get; set; }
    public Guid RoomTypeId { get; set; }
    public Rate? Rate { get; set; }
    public RoomType? RoomType { get; set; }
    public ICollection<RateTimeline> RateTimelines { get; set; } = [];
    public ICollection<ReservationRoom>? ReservationRooms { get; set; }
}
