using ACE;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using EasyNetQ.Loggers;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Yue.Bookings.Model;
using Yue.Bookings.Repository;
using Yue.Bookings.Model.Write;
using Yue.Bookings.Repository.Write;
using Yue.Common.Log;

namespace Yue.Bookings.MicroServices
{
    public class BootStrapper
    {
        public static Ninject.IKernel Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static IActionBus ActionBus { get; private set; }
        public static void BootStrap()
        {
            Container = new StandardKernel();

            EasyNetQ.RabbitHutch.SetContainerFactory(() => { return new EasyNetQ.DI.NinjectAdapter(Container); });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(ConfigurationManager.ConnectionStrings["RabbitMq"].ConnectionString,
                x => x.Register<EasyNetQ.IEasyNetQLogger, NullLogger>());

            BindFrameworkObjects();
            BindBusinessObjects();

            ActionBus = Container.GetService(typeof(IActionBus)) as IActionBus;
        }

        private static void BindFrameworkObjects()
        {
            Container.Settings.AllowNullInjection = true;
            Container.Bind<ACE.Loggers.IBusLogger>().To<Log4NetBusLogger>().InSingletonScope();
            Container.Bind<ICommandHandlerFactory>().To<CommandHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument(Constants.ParamCommandAssmblies, new string[] { "Yue.Bookings.Contract" })
                .WithConstructorArgument(Constants.ParamHandlerAssmblies, new string[] { "Yue.Bookings.Model.Write" });
            Container.Bind<ICommandBus>().To<CommandBus>().InSingletonScope();
            Container.Bind<IEventHandlerFactory>().To<EventHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument(Constants.ParamEventAssmblies, new string[] { "Yue.Bookings.ContractFS" })
                .WithConstructorArgument(Constants.ParamHandlerAssmblies, new string[] { "Yue.Bookings.Model.Write" });
            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            Container.Bind<IEventBus>().To<EventBus>().InThreadScope();
            Container.Bind<IActionHandlerFactory>().To<ActionHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument(Constants.ParamActionAssmblies, new string[] { "Yue.Bookings.ContractFS" })
                .WithConstructorArgument(Constants.ParamHandlerAssmblies, new string[] { "Yue.Bookings.Application" });
            // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
            Container.Bind<IActionBus>().To<ActionBus>().InThreadScope();
        }

        private static void BindBusinessObjects()
        {
            Yue.Common.Repository.SqlOption sqlOptionWrite =
                new Common.Repository.SqlOption
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Bookings.Write"].ConnectionString
                };

            Container.Bind<IBookingWriteRepository>().To<BookingWriteRepository>().InSingletonScope()
                .WithConstructorArgument("option", sqlOptionWrite);
        }

        public static void Dispose()
        {
            if (EasyNetQBus != null)
            {
                EasyNetQBus.Dispose();
            }
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}
