using ReservationManagementSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class ReservationRoom : BaseEntity
    {
        public int ReservationId { get; set; }
        public int RateRoomTypeId { get; set; }
        public int RoomId { get;set; } // specific Room which was assigned to a reservation
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public decimal Price { get; set; } // Sum of prices of all child ReservationRoomTimeline rows
    }
}
