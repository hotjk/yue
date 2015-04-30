using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class LeaveAMessage : BookingActionBase
    {
        public LeaveAMessage(int actionId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(actionId, resourceId, bookingId, BookingAction.LeaveAMessage, message, createBy, createAt) { }
    }
}
