using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Domain.Enums
{
    internal enum ReservationStatus
    {
        Created = 1, Reserved, CheckedIn, CheckedOut, Canceled
    }
}
