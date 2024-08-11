using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class RoomType : BaseEntity
{
    public Guid HotelId { get; set; }
    public required string Name { get; set; }
    public byte NumberOfRooms { get; set; }
    public bool IsActive { get; set; }
    public byte MinCapacity { get; set; } // Min number of guests it can accommodate
    public byte MaxCapacity { get; set; } // Max number of guests it can accommodate
    public Hotel? Hotel { get; set; }
    public ICollection<Room>? Rooms { get; set; }
    public ICollection<AvailabilityTimeline> AvailabilityTimelines { get; set; } = [];
    public ICollection<RateRoomType>? RateRoomTypes { get; set; }
}
