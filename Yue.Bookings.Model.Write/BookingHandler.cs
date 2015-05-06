using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
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

            _eventBus.Publish(new BookingInstanceCreated
            {
                BookingId = command.BookingId,
                ResourceId = command.ResourceId,
                CreateAt = command.CreateAt,
                CreateBy = command.CreateBy
            }.ToExternalQueue());
        }

        public void Execute(ConfirmSubscription command)
        {
            Booking booking = EnsourceBookingExisted(command.BookingId);
            booking.EnsoureAndUpdateState(command);

            _repository.Update(booking);
            _repository.AddAction(command);
        }

        public void Execute(CancelSubscriotion command)
        {
            Booking booking = EnsourceBookingExisted(command.BookingId);
            booking.EnsoureAndUpdateState(command);

            _repository.Update(booking);
            _repository.AddAction(command);
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
            booking.EnsoureAndUpdateState(command);
            booking.ChangeTime(command.TimeSlot);

            _repository.Update(booking);
            _repository.AddAction(command);
        }
    }
}
