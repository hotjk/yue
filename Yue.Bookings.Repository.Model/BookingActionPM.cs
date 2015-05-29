using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Commands;
using Yue.Common.Contract;

namespace Yue.Bookings.Repository.Model
{
    public class BookingActivityPM
    {
        public int ActivityId { get; private set; }
        public int ResourceId { get; private set; }
        public int BookingId { get; private set; }
        public BookingCommand Type { get; protected set; }
        public string Message { get; private set; }
        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public TimeSlot TimeSlot { get; private set; }
        public DateTime? From { get; private set; }
        public DateTime? To { get; private set; }
        public int? Minutes { get; private set; }

        private static IDictionary<BookingCommand, Type> _bookingActionTypes;
        static BookingActivityPM()
        {
            _bookingActionTypes = new Dictionary<BookingCommand, Type>();
            var ns = typeof(BookingCommandBase).Namespace;
            foreach (BookingCommand value in Enum.GetValues(typeof(BookingCommand)))
            {
                string name = Enum.GetName(typeof(BookingCommand), value);
                // Assumed that BookingCommandBase and concrete class in the same directory. 
                _bookingActionTypes.Add(value, typeof(BookingCommandBase).Assembly.GetType(ns + "." + name));
            }
            foreach (var value in _bookingActionTypes.Values)
            {
                Mapper.CreateMap(typeof(BookingActivityPM), value);
                Mapper.CreateMap(value, typeof(BookingActivityPM)).ForMember("Type", opt => opt.Ignore());
            }
        }

        public static BookingCommandBase FromPM(BookingActivityPM actionPM)
        {
            if(actionPM.From != null && actionPM.To != null)
            {
                actionPM.TimeSlot = (new TimeSlot(actionPM.From.Value, actionPM.To.Value));
            }
            
            return (Mapper.Map(actionPM, typeof(BookingActivityPM), _bookingActionTypes[actionPM.Type]) as BookingCommandBase);
        }

        public static BookingActivityPM ToPM(BookingCommandBase action)
        {
            BookingActivityPM pm = Mapper.Map<BookingActivityPM>(action);
            if (pm.TimeSlot != null)
            {
                pm.From = pm.TimeSlot.From;
                pm.To = pm.TimeSlot.To;
                pm.Minutes = pm.TimeSlot.Minutes;
            }
            pm.Type = (BookingCommand)Enum.Parse(typeof(BookingCommand), action.GetType().Name, true);
            return pm;
        }
    }
}
