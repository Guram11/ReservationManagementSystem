using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class RateRoomType
    {
        // Below IDs should form a single composite primary key
        public int RateId { get; set; }
        public int RoomTypeId { get; set; }
    }
}
