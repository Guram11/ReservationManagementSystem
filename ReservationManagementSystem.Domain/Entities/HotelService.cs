﻿using ReservationManagementSystem.Domain.Common;
using ReservationManagementSystem.Domain.Enums;

namespace ReservationManagementSystem.Domain.Entities;

public class HotelService : BaseEntity
{
    public Guid HotelId { get; set; }
    public HotelServiceTypes ServiceTypeId { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public Hotel? Hotel { get; set; }
    public ICollection<ReservationRoomServices>? ReservationRoomServices { get; set; }
}
