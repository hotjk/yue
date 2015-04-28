using ACE.Exceptions;
using Grit.Pattern.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Model
{
    public class Booking
    {
        public int ResourceId { get; private set; }
        public int BookingId { get; private set; }
        public TimeSlot TimeSlot { get; private set; }
        public BookingState State { get; private set; }

        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int UpdateBy { get; private set; }
        public DateTime UpdateAt { get; private set; }

        public IEnumerable<BookingActionBase> Actions { get; private set; }

        private static StateMachine<BookingState, BookingAction> _stateMachine;
        private static IDictionary<BookingAction, Type> _bookingActionTypes;
        
        static Booking()
        {
            InitStateMachine();
            InitMap();
        }

        private static void InitMap()
        {
            var ns = typeof(BookingActionBase).Namespace;
            _bookingActionTypes = new Dictionary<BookingAction, Type>();

            foreach (BookingAction value in Enum.GetValues(typeof(BookingAction)))
            {
                string name = Enum.GetName(typeof(BookingAction), value);
                _bookingActionTypes.Add(value, typeof(BookingAction).Assembly.GetType(ns + "." + name));
            }

            foreach (var value in _bookingActionTypes.Values)
            {
                AutoMapper.Mapper.CreateMap(typeof(BookingActionBase), value);
            }
        }

        private static void InitStateMachine()
        {
            _stateMachine = new StateMachine<BookingState, BookingAction>();
            _stateMachine.Configure(BookingState.Initial)
                .Permit(BookingAction.SubscribeResource, BookingState.Subscribed);
            _stateMachine.Configure(BookingState.Subscribed)
                .Permit(BookingAction.CancelSubscriotion, BookingState.Canceled)
                .Permit(BookingAction.ChangeTime, BookingState.Subscribed)
                .Permit(BookingAction.ConfirmSubscription, BookingState.Confirmed)
                .Permit(BookingAction.LeaveAMessage, BookingState.Subscribed);
            _stateMachine.Configure(BookingState.Confirmed)
                .Permit(BookingAction.CancelSubscriotion, BookingState.Canceled)
                .Permit(BookingAction.ChangeTime, BookingState.Subscribed)
                .Permit(BookingAction.LeaveAMessage, BookingState.Confirmed);
            _stateMachine.Configure(BookingState.Canceled)
                .Permit(BookingAction.LeaveAMessage, BookingState.Canceled);
        }

        public void EnsoureAndUpdateState(BookingActionBase action)
        {
            if (!_stateMachine.Instance(this.State).Fire(action.Type))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid booking state.");
            }
            this.UpdateBy = action.CreateBy;
            this.UpdateAt = action.CreateAt;
        }

        public static Booking Create(SubscribeResource action)
        {
            Booking booking = new Booking();
            booking.State = BookingState.Initial;

            booking.EnsoureAndUpdateState(action);
            booking.BookingId = action.BookingId;
            booking.ResourceId = action.ResourceId;
            booking.TimeSlot = action.TimeSlot;
            booking.CreateBy = action.CreateBy;
            booking.CreateAt = action.CreateAt;
            return booking;
        }

        public void ChangeTime(TimeSlot timeSlot)
        {
            this.TimeSlot = TimeSlot;
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
