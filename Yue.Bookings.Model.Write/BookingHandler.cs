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
        static BookingHandler()
        {
            AutoMapper.Mapper.CreateMap<Booking, BookingIsCreated>();
            AutoMapper.Mapper.CreateMap<Booking, BookingStateChanged>();
            AutoMapper.Mapper.CreateMap<Booking, BookingTimeChanged>();
        }

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

            _eventBus.Publish(AutoMapper.Mapper.Map<BookingIsCreated>(booking).ToExternalQueue());
        }

        public void Execute(ConfirmSubscription command)
        {
            Booking booking = EnsourceBookingExisted(command.BookingId);
            var orignalState = booking.State;
            booking.EnsoureAndUpdateState(command);

            _repository.Update(booking);
            _repository.AddAction(command);

            var evt = AutoMapper.Mapper.Map<BookingStateChanged>(booking);;
            evt.OrignalState = orignalState;
            _eventBus.Publish(evt.ToExternalQueue());
        }

        public void Execute(CancelSubscriotion command)
        {
            Booking booking = EnsourceBookingExisted(command.BookingId);
            var orignalState = booking.State;
            booking.EnsoureAndUpdateState(command);

            _repository.Update(booking);
            _repository.AddAction(command);

            var evt = AutoMapper.Mapper.Map<BookingStateChanged>(booking);
            evt.OrignalState = orignalState;
            _eventBus.Publish(evt.ToExternalQueue());
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

            var evt = AutoMapper.Mapper.Map<BookingTimeChanged>(booking);
            evt.OrignalTimeSlot = orignalTimeSlot;
            _eventBus.Publish(evt.ToExternalQueue());
        }
    }
}
