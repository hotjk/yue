using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class ConfirmSubscription : BookingActionBase
    {
        public ConfirmSubscription()
        {
            this.Type = BookingAction.ConfirmSubscription;
        }
    }
}
