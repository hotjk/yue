using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Application
{
    public class BookingApplication : 
        IActionHandler<SubscribeResource>,
        IActionHandler<ConfirmSubscription>,
        IActionHandler<CancelSubscriotion>,
        IActionHandler<LeaveAMessage>,
        IActionHandler<ChangeTime>
    {
        protected ICommandBus CommandBus { get; private set; }
        protected IEventBus EventBus { get; private set; }

        public BookingApplication(ICommandBus commandBus, IEventBus eventBus)
        {
            this.CommandBus = commandBus;
            this.EventBus = eventBus;
        }

        public void Invoke(SubscribeResource action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Contract.Commands.SubscribeResource(
                    action.ActivityId, action.ResourceId, action.BookingId, action.Message, action.TimeSlot, action.CreateAt, action.CreateBy));
                unitOfwork.Complete();
            }
        }

        public void Invoke(ConfirmSubscription action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Contract.Commands.ConfirmSubscription(
                    action.ActivityId, action.ResourceId, action.BookingId, action.Message, action.CreateAt, action.CreateBy));
                unitOfwork.Complete();
            }
        }

        public void Invoke(CancelSubscriotion action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Contract.Commands.CancelSubscriotion(
                    action.ActivityId, action.ResourceId, action.BookingId, action.Message, action.CreateAt, action.CreateBy));
                unitOfwork.Complete();
            }
        }

        public void Invoke(LeaveAMessage action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Contract.Commands.LeaveAMessage(
                    action.ActivityId, action.ResourceId, action.BookingId, action.Message, action.CreateAt, action.CreateBy));
                unitOfwork.Complete();
            }
        }

        public void Invoke(ChangeTime action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Contract.Commands.ChangeTime(
                    action.ActivityId, action.ResourceId, action.BookingId, action.Message, action.TimeSlot, action.CreateAt, action.CreateBy));
                unitOfwork.Complete();
            }
        }
    }
}
