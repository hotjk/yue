using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract
{
    public enum BookingState
    {
        Initial = 0,
        Subscribed = 1,
        Confirmed = 2,
        Canceled = 9,
    }
}
