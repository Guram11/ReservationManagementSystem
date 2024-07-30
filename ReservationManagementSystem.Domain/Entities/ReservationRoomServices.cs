using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class ReservationRoomServices
    {
        // Below IDs should form a composite primary key
        public int HotelServiceId { get; set; }
        public int ReservationRoomId { get; set; }
    }
}
