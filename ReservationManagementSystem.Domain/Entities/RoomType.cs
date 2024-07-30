using ReservationManagementSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class RoomType : BaseEntity
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public byte NumberOfRooms { get; set; }
        public bool IsActive { get; set; }
        public byte MinCapacity { get; set; } // min number of guests it can accomodate
        public byte MaxCapacity { get; set; }// max number of guests it can accomodate
    }
}
