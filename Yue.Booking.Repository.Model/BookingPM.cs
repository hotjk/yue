using AutoMapper;
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
    public class BookingPM : Booking
    {
        private static IDictionary<BookingAction, Type> _bookingActionTypes;

        static BookingPM()
        {
            InitMapper();
        }

        private static void InitMapper()
        {
            Mapper.CreateMap<BookingPM, Booking>();

            var ns = typeof(BookingActionBase).Namespace;
            _bookingActionTypes = new Dictionary<BookingAction, Type>();

            foreach (BookingAction value in Enum.GetValues(typeof(BookingAction)))
            {
                string name = Enum.GetName(typeof(BookingAction), value);
                _bookingActionTypes.Add(value, typeof(BookingAction).Assembly.GetType(ns + "." + name));
            }

            foreach (var value in _bookingActionTypes.Values)
            {
                Mapper.CreateMap(typeof(BookingActionPM), value);
            }
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public int Minutes { get; private set; }

        public Booking Refine()
        {
            this.TimeSlot = new Contract.TimeSlot(From, To);
            Booking booking =  Mapper.Map<Booking>(this);
            booking.ChangeTime(new TimeSlot(From, To));
            return booking;
        }

        public void RefineActions(IEnumerable<BookingActionPM> actions)
        {
            Actions = actions.Select(n =>
            {
                return (Mapper.Map(n, typeof(BookingActionPM), _bookingActionTypes[n.Type]) as BookingActionBase);
            });
        }
    }
}
