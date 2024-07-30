using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class ReservationRoomTimeline
    {
        public int ReservationRoomId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Price { get; set; }
    }
}
