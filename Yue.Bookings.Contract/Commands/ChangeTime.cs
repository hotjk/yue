using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Commands
{
    public class ChangeTime : BookingCommandBase, ACE.ICommand
    {
        public ChangeTime(int activityId, int resourceId, int bookingId, string message, int createBy, DateTime createAt, TimeSlot timeSlot)
            : base(activityId,resourceId, bookingId, BookingCommand.ChangeTime, message, createBy, createAt)
        {
            this.TimeSlot = timeSlot;
        }

        public TimeSlot TimeSlot { get; private set; }
    }
}
