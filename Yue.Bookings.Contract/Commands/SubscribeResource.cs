using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Commands
{
    public class SubscribeResource : BookingCommandBase, ACE.ICommand
    {
        public SubscribeResource(int actionId, int resourceId, int bookingId, string message, int createBy, DateTime createAt, TimeSlot timeSlot)
            : base(actionId, resourceId, bookingId, BookingCommand.SubscribeResource, message, createBy, createAt) 
        {
            this.TimeSlot = timeSlot;
        }

        public TimeSlot TimeSlot { get; private set; }
    }
}
