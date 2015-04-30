using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Bookings.Contract.Actions
{
    public class BookingActionBase : ACE.Action, ACE.ICommand
    {
        public BookingActionBase() { }
        public BookingActionBase(int actionId, int resourceId, int bookingId, BookingAction type, string message, int createBy, DateTime createAt)
        {
            this.ActionId = actionId;
            this.ResourceId = resourceId;
            this.BookingId = bookingId;
            this.Type = type;
            this.Message = message;
            this.CreateBy = createBy;
            this.CreateAt = createAt;
        }
        public int ActionId { get; private set; }
        public int ResourceId { get; private set; }
        public int BookingId { get; private set; }
        public BookingAction Type { get; protected set; }
        public string Message { get; private set; }
        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
    }
}
