using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Model
{
    public class Booking
    {
        public int ResourceId { get; set; }
        public int BookingId { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public BookingState State { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }

        public ICollection<BookingActionBase> Actions { get; set; }
    }
}
