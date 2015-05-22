using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Model.Commands;
using Yue.Bookings.Contract.Events;
using Yue.Common.Contract;

namespace Yue.Bookings.Model.Write
{
    public class BookingHandler : 
        ICommandHandler<SubscribeResource>,
        ICommandHandler<ConfirmSubscription>,
        ICommandHandler<CancelSubscriotion>,
        ICommandHandler<LeaveAMessage>,
        ICommandHandler<ChangeTime>
    {
        private IEventBus _eventBus;
        private IBookingWriteRepository _repository;

        public BookingHandler(IEventBus eventBus, IBookingWriteRepository repository)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        private void EnsoureBookingNotExisted(int bookingId)
        {
            Booking booking = _repository.GetForUpdate(bookingId);
            if (booking != null)
            {
                throw new BusinessException(BusinessStatusCode.Conflict, "Booking already existed.");
            }
        }
        private Booking EnsourceBookingExisted(int bookingId)
        {
            Booking booking = _repository.GetForUpdate(bookingId);
            if (booking == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "Booking not found.");
            }
            return booking;
        }

        public void Execute(SubscribeResource command)
        {
            EnsoureBookingNotExisted(command.BookingId);
            Booking booking = Booking.Create(command);

            _repository.Add(booking);
            _repository.AddAction(command);

            var evt = new BookingIsCreated(booking.ResourceId, booking.BookingId, booking.State, booking.TimeSlot, booking.UpdateAt, booking.UpdateBy);
            _eventBus.Publish(evt);
        }

        public void Execute(ConfirmSubscription command)
        {
            Booking booking = EnsourceBookingExisted(command.BookingId);
            var orignalState = booking.State;
            booking.EnsoureAndUpdateState(command);

            _repository.Update(booking);
            _repository.AddAction(command);

            var evt = new BookingStateChanged(orignalState, booking.State, booking.UpdateAt, booking.UpdateBy);
            _eventBus.Publish(evt);
        }

        public void Execute(CancelSubscriotion command)
        {
            Booking booking = EnsourceBookingExisted(command.BookingId);
            var orignalState = booking.State;
            booking.EnsoureAndUpdateState(command);

            _repository.Update(booking);
            _repository.AddAction(command);

            var evt = new BookingStateChanged(orignalState, booking.State, booking.UpdateAt, booking.UpdateBy);
            _eventBus.Publish(evt);
        }

        public void Execute(LeaveAMessage command)
        {
            Booking booking = _repository.GetForUpdate(command.BookingId);
            booking.EnsoureAndUpdateState(command);

            _repository.AddAction(command);
        }

        public void Execute(ChangeTime command)
        {
            Booking booking = _repository.GetForUpdate(command.BookingId);
            var orignalTimeSlot = booking.TimeSlot.Clone();
            booking.EnsoureAndUpdateState(command);
            booking.ChangeTime(command.TimeSlot);

            _repository.Update(booking);
            _repository.AddAction(command);

            var evt = new BookingTimeChanged(orignalTimeSlot, booking.TimeSlot, booking.UpdateAt, booking.UpdateBy);
            _eventBus.Publish(evt);
        }
    }
}
