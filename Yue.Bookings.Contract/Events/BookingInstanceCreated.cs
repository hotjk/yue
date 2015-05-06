using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Events
{
    public class BookingInstanceCreated : ACE.Event
    {
        public int ResourceId { get; set; }
        public int BookingId { get; set; }
        public BookingState State { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
