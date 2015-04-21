using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Model
{
    public class Booking
    {
        public int ResourceId { get; set; }
        public int BookingId { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
