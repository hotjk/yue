using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class ConfirmSubscription : BookingActionBase
    {
        public ConfirmSubscription(int actionId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(actionId, resourceId, bookingId, BookingAction.ConfirmSubscription, message, createBy, createAt) { }
    }
}
