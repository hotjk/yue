using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Model
{
    public interface IBookingRepository
    {
        Booking Get(int bookingId);
        bool InsertBooking(Booking booking);
        bool UpdateBooking(Booking booking);
        IEnumerable<Booking> BookingsByResource(TimeSlot timeSlot, int resourceId);
    }
}
