using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Commands
{
    public class ConfirmSubscription : BookingCommandBase, ACE.ICommand
    {
        public ConfirmSubscription(int activityId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(activityId, resourceId, bookingId, BookingCommand.ConfirmSubscription, message, createBy, createAt) { }
    }
}
