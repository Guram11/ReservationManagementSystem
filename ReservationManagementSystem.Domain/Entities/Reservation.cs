using ReservationManagementSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class Reservation : BaseEntity
    {
        public int HotelId { get; set; }
        public string Number { get; set; } // this should be some randomly generated unique string
        public decimal Price { get; set; }
        public byte StatusId { get; set; } // this is coming from ReservationStatus enum
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public string Currency { get; set; } // TODO: create enum for GEL, USD, EUR
    }
}
