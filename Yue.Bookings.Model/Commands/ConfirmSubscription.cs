using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;

namespace Yue.Bookings.Model.Commands
{
    public class ConfirmSubscription : BookingCommandBase, ACE.ICommand
    {
        public ConfirmSubscription(int activityId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(activityId, resourceId, bookingId, BookingCommand.ConfirmSubscription, message, createBy, createAt) { }
    }
}
