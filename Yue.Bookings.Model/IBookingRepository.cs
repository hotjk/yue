using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Model
{
    public interface IBookingRepository
    {
        Booking Get(int bookingId);
        
        IEnumerable<Booking> BookingsByResource(TimeSlot timeSlot, int resourceId);
    }
}
