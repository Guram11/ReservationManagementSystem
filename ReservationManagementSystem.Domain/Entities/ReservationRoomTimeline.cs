﻿using ReservationManagementSystem.Domain.Common;

namespace ReservationManagementSystem.Domain.Entities;

public class ReservationRoomTimeline : BaseEntity
{
    public Guid ReservationRoomId { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public ReservationRoom? ReservationRoom { get; set; }
}
