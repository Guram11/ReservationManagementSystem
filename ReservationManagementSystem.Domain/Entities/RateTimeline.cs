using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class RateTimeline
    {
        public DateOnly Date { get; set; }
        public int RateRoomTypeId { get; set; }
        public decimal Price { get; set; }
    }
}
