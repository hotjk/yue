using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;

namespace Yue.Bookings.MicroServices
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();

            BootStrapper.ActionStation.SubscribeInParallel<BookingActionBase>(10);

            Console.WriteLine("Ctrl-C to exit");
            Console.CancelKeyPress += (source, cancelKeyPressArgs) =>
            {
                Console.WriteLine("Shut down, please wait...");
                BootStrapper.Dispose();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("Shut down completed");
            };

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
