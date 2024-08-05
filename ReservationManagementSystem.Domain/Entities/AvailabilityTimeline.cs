using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class AvailabilityTimeline : BaseEntity
{
    public DateOnly Date { get; set; }
    public Guid RoomTypeId { get; set; }
    public byte Available { get; set; }
    public RoomType? RoomType { get; set; }
}
 