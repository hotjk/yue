using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class BookingActionVM
    {
        public int ActionId { get; private set; }
        public int ResourceId { get; private set; }
        public int BookingId { get; private set; }
        public BookingCommand Type { get; protected set; }
        public string Message { get; private set; }
        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
    }
}
