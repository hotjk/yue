using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;

namespace Yue.Bookings.Model.Commands
{
    public class SubscribeResource : BookingCommandBase, ACE.ICommand
    {
        public SubscribeResource(int activityId, int resourceId, int bookingId, string message, int createBy, DateTime createAt, TimeSlot timeSlot)
            : base(activityId, resourceId, bookingId, BookingCommand.SubscribeResource, message, createBy, createAt) 
        {
            this.TimeSlot = timeSlot;
        }

        public TimeSlot TimeSlot { get; private set; }
    }
}
