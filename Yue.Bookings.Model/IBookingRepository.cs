using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Model
{
    public interface IBookingRepository
    {
        Booking Get(int bookingId);
        bool Add(Booking booking);
        bool Update(Booking booking, BookingActionBase action);
        IEnumerable<Booking> BookingsByResource(TimeSlot timeSlot, int resourceId);
    }
}
