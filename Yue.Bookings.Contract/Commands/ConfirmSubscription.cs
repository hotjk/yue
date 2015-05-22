using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Commands
{
    public class ConfirmSubscription : BookingCommandBase, ACE.ICommand
    {
        public ConfirmSubscription(int actionId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(actionId, resourceId, bookingId, BookingCommand.ConfirmSubscription, message, createBy, createAt) { }
    }
}
