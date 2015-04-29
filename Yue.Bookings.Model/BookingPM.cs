using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Common.Contract;

namespace Yue.Bookings.Model
{
    public class BookingPM : Booking, IPersistenceModel
    {
        private static IDictionary<BookingAction, Type> _bookingActionTypes;

        static BookingPM()
        {
            InitMap();
        }

        private static void InitMap()
        {
            var ns = typeof(BookingActionPM).Namespace;
            _bookingActionTypes = new Dictionary<BookingAction, Type>();

            foreach (BookingAction value in Enum.GetValues(typeof(BookingAction)))
            {
                string name = Enum.GetName(typeof(BookingAction), value);
                _bookingActionTypes.Add(value, typeof(BookingAction).Assembly.GetType(ns + "." + name));
            }

            foreach (var value in _bookingActionTypes.Values)
            {
                AutoMapper.Mapper.CreateMap(typeof(BookingActionPM), value);
            }
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public int Minutes { get; private set; }

        public void Persist()
        {
            From = TimeSlot.From;
            To = TimeSlot.To;
            Minutes = TimeSlot.Minutes;
        }

        public void Rehydrate()
        {
            this.TimeSlot = new Contract.TimeSlot(From, To);
        }

        public void InitalActions(IEnumerable<BookingActionBase> actions)
        {
            Actions = actions.Select(n =>
            {
                return (AutoMapper.Mapper.Map(n, typeof(BookingActionBase), _bookingActionTypes[n.Type]) as BookingActionBase);
            });
        }
    }
}
