using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Commands
{
    public class LeaveAMessage : BookingCommandBase, ACE.ICommand
    {
        public LeaveAMessage(int actionId, int resourceId, int bookingId, string message, int createBy, DateTime createAt)
            : base(actionId, resourceId, bookingId, BookingCommand.LeaveAMessage, message, createBy, createAt) { }
    }
}
