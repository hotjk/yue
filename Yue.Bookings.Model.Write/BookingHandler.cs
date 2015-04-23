using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.Model.Write
{
    public class BookingHandler : ACE.ICommandHandler<SubscribeResource>
    {
        public void Execute(SubscribeResource command)
        {
            throw new NotImplementedException();
        }
    }
}
