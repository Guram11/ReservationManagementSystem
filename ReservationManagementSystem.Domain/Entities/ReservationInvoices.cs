using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Entities
{
    // TODO: add CreateDate/UpdateDate (auditing) fields to BaseEntity
    internal class ReservationInvoices
    {
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public decimal Paid { get; set; }
        public decimal Due { get; set; }
        public string Currency { get; set; }
    }
}
