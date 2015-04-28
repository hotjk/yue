using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Model
{
    public class BookingService : IBookingService
    {
        private IBookingRepository _repository;

        public BookingService(IBookingRepository repository)
        {
            _repository = repository;
        }
        public Booking Get(int bookingId, bool withActions)
        {
            Booking booking = _repository.Get(bookingId, withActions);
            return booking;
        }

        public IEnumerable<Booking> BookingsByResource(Contract.TimeSlot timeSlot, int resourceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Booking> BookingsByUser(Contract.TimeSlot timeSlot, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
