using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Model
{
    public class TimeSlot
    {
        public TimeSlot(DateTime from, DateTime to)
        {
            if (from > to)
            {
                throw new ArgumentException("To MUST >= From.");
            }
            this.From = from;
            this.To = to;
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public int Minutes
        {
            get
            {
                return (To - From).Minutes;
            }
        }
    }
}
