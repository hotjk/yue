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
                    new Yue.Bookings.Model.Commands.SubscribeResource(
                    action.ActionId,action.ResourceId, action.BookingId, action.Message, action.CreateBy, action.CreateAt, action.TimeSlot));
                unitOfwork.Complete();
            }
        }

        public void Invoke(ConfirmSubscription action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Model.Commands.ConfirmSubscription(
                    action.ActionId,action.ResourceId, action.BookingId, action.Message, action.CreateBy, action.CreateAt));
                unitOfwork.Complete();
            }
        }

        public void Invoke(CancelSubscriotion action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Model.Commands.CancelSubscriotion(
                    action.ActionId, action.ResourceId, action.BookingId, action.Message, action.CreateBy, action.CreateAt));
                unitOfwork.Complete();
            }
        }

        public void Invoke(LeaveAMessage action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Model.Commands.LeaveAMessage(
                    action.ActionId, action.ResourceId, action.BookingId, action.Message, action.CreateBy, action.CreateAt));
                unitOfwork.Complete();
            }
        }

        public void Invoke(ChangeTime action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CommandBus.Send(
                    new Yue.Bookings.Model.Commands.ChangeTime(
                    action.ActionId, action.ResourceId, action.BookingId, action.Message, action.CreateBy, action.CreateAt, action.TimeSlot));
                unitOfwork.Complete();
            }
        }
    }
}
