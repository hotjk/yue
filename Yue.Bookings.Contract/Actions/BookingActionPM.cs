using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;

namespace Yue.Bookings.Contract.Actions
{
    public class BookingActionPM : BookingActionBase, IPersistenceModel
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public int Minutes { get; private set; }

        public void Persist()
        {
            From = TimeSlot.From;
            To = TimeSlot.To;
            Minutes = TimeSlot.Minutes;
        }

        public void Rehydrate()
        {
            this.TimeSlot = new Contract.TimeSlot(From, To);
        }
    }
}
