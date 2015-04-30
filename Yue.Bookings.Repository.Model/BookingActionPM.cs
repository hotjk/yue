using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Common.Contract;

namespace Yue.Bookings.Repository.Model
{
    public class BookingActionPM : BookingActionBase
    {
        public TimeSlot TimeSlot { get; private set; }
        public DateTime? From { get; private set; }
        public DateTime? To { get; private set; }
        public int? Minutes { get; private set; }

        private static IDictionary<BookingAction, Type> _bookingActionTypes;
        static BookingActionPM()
        {
            _bookingActionTypes = new Dictionary<BookingAction, Type>();
            var ns = typeof(BookingActionBase).Namespace;
            foreach (BookingAction value in Enum.GetValues(typeof(BookingAction)))
            {
                string name = Enum.GetName(typeof(BookingAction), value);
                // Assumed that BookingActionBase and concrete class in the same directory. 
                _bookingActionTypes.Add(value, typeof(BookingAction).Assembly.GetType(ns + "." + name));
            }
            foreach (var value in _bookingActionTypes.Values)
            {
                Mapper.CreateMap(typeof(BookingActionPM), value);
                Mapper.CreateMap(value, typeof(BookingActionPM));
            }
        }

        public static BookingActionBase FromPM(BookingActionPM actionPM)
        {
            if(actionPM.From != null && actionPM.To != null)
            {
                actionPM.TimeSlot = (new TimeSlot(actionPM.From.Value, actionPM.To.Value));
            }
            
            return (Mapper.Map(actionPM, typeof(BookingActionPM), _bookingActionTypes[actionPM.Type]) as BookingActionBase);
        }

        public static BookingActionPM ToPM(BookingActionBase action)
        {
            BookingActionPM pm = Mapper.Map<BookingActionPM>(action);
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
