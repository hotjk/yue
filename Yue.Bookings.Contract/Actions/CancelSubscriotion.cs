using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class CancelSubscriotion : BookingActionBase
    {
        public CancelSubscriotion()
        {
            this.Type = BookingAction.CancelSubscriotion;
        }
    }
}
