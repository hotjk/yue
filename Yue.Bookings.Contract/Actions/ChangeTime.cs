using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class ChangeTime : BookingActionBase, ACE.ICommand
    {
        public ChangeTime(int actionId, int resourceId, int bookingId, string message, int createBy, DateTime createAt, TimeSlot timeSlot)
            : base(actionId,resourceId, bookingId, BookingAction.ChangeTime, message, createBy, createAt)
        {
            this.TimeSlot = timeSlot;
        }

        public TimeSlot TimeSlot { get; private set; }
    }
}
