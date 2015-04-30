using ACE.Exceptions;
using Grit.Pattern.FSM;
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
    public class Booking
    {
        public int ResourceId { get; private set; }
        public int BookingId { get; private set; }
        public TimeSlot TimeSlot { get; protected set; }
        public BookingState State { get; private set; }

        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int UpdateBy { get; private set; }
        public DateTime UpdateAt { get; private set; }

        public IEnumerable<BookingActionBase> Actions { get; protected set; }

        private static StateMachine<BookingState, BookingAction> _stateMachine;
        
        static Booking()
        {
            InitStateMachine();
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
    }
}
