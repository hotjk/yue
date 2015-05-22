using ACE.Exceptions;
using Grit.Pattern.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Commands;
using Yue.Common.Contract;

namespace Yue.Bookings.Model
{
    public class Booking : IAggregateRoot
    {
        public int ResourceId { get; private set; }
        public int BookingId { get; private set; }
        public TimeSlot TimeSlot { get; protected set; }
        public BookingState State { get; private set; }

        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int UpdateBy { get; private set; }
        public DateTime UpdateAt { get; private set; }

        public IEnumerable<BookingCommandBase> Activities { get; protected set; }

        private static StateMachine<BookingState, BookingCommand> _stateMachine;
        
        static Booking()
        {
            InitStateMachine();
        }

        private static void InitStateMachine()
        {
            _stateMachine = new StateMachine<BookingState, BookingCommand>();
            _stateMachine.Configure(BookingState.Initial)
                .Permit(BookingCommand.SubscribeResource, BookingState.Subscribed);
            _stateMachine.Configure(BookingState.Subscribed)
                .Permit(BookingCommand.CancelSubscriotion, BookingState.Canceled)
                .Permit(BookingCommand.ChangeTime, BookingState.Subscribed)
                .Permit(BookingCommand.ConfirmSubscription, BookingState.Confirmed)
                .Permit(BookingCommand.LeaveAMessage, BookingState.Subscribed);
            _stateMachine.Configure(BookingState.Confirmed)
                .Permit(BookingCommand.CancelSubscriotion, BookingState.Canceled)
                .Permit(BookingCommand.ChangeTime, BookingState.Subscribed)
                .Permit(BookingCommand.LeaveAMessage, BookingState.Confirmed);
            _stateMachine.Configure(BookingState.Canceled)
                .Permit(BookingCommand.LeaveAMessage, BookingState.Canceled);
        }

        public bool EnsoureState(BookingCommand bookingAction)
        {
            return _stateMachine.Instance(this.State).CanFire(bookingAction);
        }

        public void EnsoureAndUpdateState(BookingCommandBase action)
        {
            var instance = _stateMachine.Instance(this.State);
            if (!instance.Fire(action.Type))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid booking state.");
            }
            this.State = instance.State;
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
