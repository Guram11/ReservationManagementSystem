using ReservationManagementSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class AvailabilityTimeline : BaseEntity
    {
        public DateOnly Date { get; set; }
        public int RoomTypeId { get; set; }
        public byte Available { get; set; }
    }
}
