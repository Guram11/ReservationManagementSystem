using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class Room : BaseEntity
{
    public Guid RoomTypeId { get; set; }
    public required string Number { get; set; }
    public byte Floor { get; set; }
    public string? Note { get; set; }
    public RoomType? RoomType { get; set; }
    public ICollection<ReservationRoom>? ReservationRooms { get; set; }
}
