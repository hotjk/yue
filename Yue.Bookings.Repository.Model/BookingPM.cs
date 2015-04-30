using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Yue.Common.Contract;

namespace Yue.Bookings.Repository.Model
{
    public class BookingPM : Booking
    {
        static BookingPM()
        {
            Mapper.CreateMap<BookingPM, Booking>();
            Mapper.CreateMap<Booking, BookingPM>();
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public int Minutes { get; private set; }

        public static Booking FromPM(BookingPM bookingPM, IEnumerable<BookingActionPM> actions = null)
        {
            bookingPM.TimeSlot = (new TimeSlot(bookingPM.From, bookingPM.To));
            if (actions != null)
            {
                bookingPM.Actions = actions.Select(n => BookingActionPM.FromPM(n));
            }
            return Mapper.Map<Booking>(bookingPM);
        }

        public static BookingPM ToPM(Booking booking)
        {
            var pm = Mapper.Map<BookingPM>(booking);
            if (pm.TimeSlot != null)
            {
                pm.From = pm.TimeSlot.From;
                pm.To = pm.TimeSlot.To;
                pm.Minutes = pm.TimeSlot.Minutes;
            }
            return pm;
        }
    }
}
