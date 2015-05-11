using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Events
{
    public class BookingStateChanged : BookingIsCreated
    {
        public BookingState OrignalState { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
