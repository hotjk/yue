using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Model.Write
{
    public interface IBookingWriteRepository
    {
        Booking GetForUpdate(int bookingId);
        bool Add(Booking booking);
        bool Update(Booking booking);
        bool AddAction(BookingActionBase action);
    }
}
