using ReservationManagementSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class Room : BaseEntity
    {
        public int RoomTypeId { get; set; }
        public string Number { get; set; }
        public byte Floor { get; set; }
        public string Note { get; set; }
    }
}
