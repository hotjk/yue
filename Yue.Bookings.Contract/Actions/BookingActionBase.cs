using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class BookingActionBase : ACE.Action, ACE.ICommand
    {
        public int ResourceId { get; set; }
        public BookingAction Type { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
