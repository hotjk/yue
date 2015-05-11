using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract
{
    public enum BookingCommand
    {
        SubscribeResource = 0,
        ConfirmSubscription = 1,
        CancelSubscriotion = 2,
        LeaveAMessage = 3,
        ChangeTime = 4
    }
}
