using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract
{
    public enum BookingState
    {
        Subscribed = 0,
        Confirmed = 1,
        Canceled = 9,
    }
}
