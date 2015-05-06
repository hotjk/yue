using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Events
{
    public class BookingTimeChanged : BookingInstanceCreated
    {
        public TimeSlot OrignalTimeSlot { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
