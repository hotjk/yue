using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class BookingActionBase : ACE.Action, ACE.ICommand
    {
        public int ResourceId { get; private set; }
        public int BookingId { get; private set; }
        public BookingAction Type { get; protected set; }
        public string Message { get; private set; }
        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public TimeSlot TimeSlot { get; protected set; }
    }
}
