using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Common.Contract;

namespace Yue.Booking.Repository.Model
{
    public class BookingActionPM : BookingActionBase
    {
        public BookingActionPM(int resourceId, int bookingId, BookingAction type, string message, int createBy, DateTime createAt)
            : base(resourceId, bookingId, type, message, createBy, createAt) { }

        public TimeSlot TimeSlot { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public int Minutes { get; private set; }
    }
}
