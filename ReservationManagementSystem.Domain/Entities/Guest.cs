using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class Guest : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public Guid ReservationRoomId { get; set; }

    public ReservationRoom? ReservationRoom { get; set; }
}
