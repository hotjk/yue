using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;

namespace Yue.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Booking booking = new Booking();

            BookingActionBase action = new BookingActionBase
            {
                Type = Bookings.Contract.BookingAction.ConfirmSubscription,
                BookingId = 1,
                CreateAt = DateTime.Now,
                CreateBy = 1,
                Message = "test",
                ResourceId = 123
            };
            System.Console.WriteLine(action);

            booking.InitalActions(new BookingActionBase[] { action });
            System.Console.WriteLine(booking.Actions.First());
        }
    }
}
