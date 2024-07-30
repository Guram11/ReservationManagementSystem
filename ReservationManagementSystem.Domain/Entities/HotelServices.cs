using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    internal class HotelServices
    {
        public int HotelId { get; set; }
        public int TypeId { get; set; } // TODO: this should come from HotelServiceTypes enum (such as Extra Bed, Hotel Damage, Mini bar, etc)
        public string Description { get; set; }
        public decimal Price;
    }
}
