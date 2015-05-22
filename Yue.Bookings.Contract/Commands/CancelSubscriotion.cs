using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Commands
{
    public class CancelSubscriotion : BookingCommandBase, ACE.ICommand
    {
        public CancelSubscriotion(int actionId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(actionId, resourceId, bookingId, BookingCommand.CancelSubscriotion, message, createBy, createAt) { }
    }
}
