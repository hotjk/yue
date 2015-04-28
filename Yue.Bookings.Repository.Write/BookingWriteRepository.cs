using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model.Write;

namespace Yue.Bookings.Repository.Write
{
    public class BookingWriteRepository : IBookingWriteRepository
    {
        public Model.Booking GetForUpdate(int bookingId)
        {
            throw new NotImplementedException();
        }

        public bool Add(Model.Booking booking)
        {
            throw new NotImplementedException();
        }

        public bool Update(Model.Booking booking)
        {
            throw new NotImplementedException();
        }

        public bool AddAction(BookingActionBase action)
        {
            throw new NotImplementedException();
        }
    }
}
