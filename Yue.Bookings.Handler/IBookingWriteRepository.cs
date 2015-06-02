using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Commands;
using Yue.Bookings.Model;

namespace Yue.Bookings.Handler
{
    public interface IBookingWriteRepository
    {
        Booking GetForUpdate(int bookingId);
        bool Add(Booking booking);
        bool Update(Booking booking);
        bool AddAction(BookingCommandBase action);
    }
}
