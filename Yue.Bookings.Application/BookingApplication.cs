using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Application
{
    public class BookingApplication : IActionHandler<SubscribeResource>
    {
        public void Invoke(SubscribeResource action)
        {
            throw new NotImplementedException();
        }
    }
}
