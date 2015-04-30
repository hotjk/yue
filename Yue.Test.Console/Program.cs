using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Yue.Bookings.Model.Write;
using Yue.Bookings.Repository;
using Yue.Bookings.Repository.Write;
using Yue.Common.Repository;

namespace Yue.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Subscribe();
        }

        private static void GetBooking()
        {
            SqlOption option = new SqlOption { ConnectionString = "Server=localhost;Port=3306;Database=yue;Uid=root;Pwd=123456;" };
            BookingRepository bookingRepository = new BookingRepository(option);
            BookingService bookingService = new BookingService(bookingRepository);
            var booking = bookingService.Get(100, true);
        }

        private static void Subscribe()
        {
            SubscribeResource sr = new SubscribeResource(1, 1, 100, "test", 1000, DateTime.Now, new Bookings.Contract.TimeSlot(DateTime.Now, DateTime.Now.AddHours(4).AddSeconds(32)));

            SqlOption option = new SqlOption { ConnectionString = "Server=localhost;Port=3306;Database=yue;Uid=root;Pwd=123456;" };
            BookingWriteRepository bookingWriteRepository = new BookingWriteRepository(option);
            BookingHandler bookingHandler = new BookingHandler(bookingWriteRepository);
            bookingHandler.Execute(sr);
        }
    }
}
