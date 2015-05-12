using ACE;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Yue.Bookings.Model;
using Yue.Bookings.Repository;
using Yue.Users.Model;
using Yue.Users.Repository;

namespace Yue.WebApi
{
    public class BootStrapper
    {
        public static Ninject.IKernel Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static void BootStrap()
        {
            Container = new StandardKernel();

            EasyNetQ.RabbitHutch.SetContainerFactory(() => { return new EasyNetQ.DI.NinjectAdapter(Container); });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(ConfigurationManager.ConnectionStrings["RabbitMq"].ConnectionString,
                x => x.Register<EasyNetQ.IEasyNetQLogger, EasyNetQ.Loggers.NullLogger>());

            BindFrameworkObjects();
            BindBusinessObjects();
        }

        private static void BindFrameworkObjects()
        {
            Container.Settings.AllowNullInjection = true;
            Container.Bind<ACE.Loggers.IBusLogger>().To<Log4NetBusLogger>().InSingletonScope();

            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            Container.Bind<IEventBus>().To<EventBus>()
                .InThreadScope()
                .WithConstructorArgument(Constants.ParamEventDistributionOptions, ACE.Event.EventDistributionOptions.Queue);
            
            // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
            Container.Bind<IActionBus>().To<ActionBus>().InThreadScope()
                .WithConstructorArgument(Constants.ParamActionShouldDistributeToExternalQueue, true);
        }

        private static void BindBusinessObjects()
        {
            Container.Bind<ISequenceRepository>().To<SequenceRepository>().InSingletonScope()
                .WithConstructorArgument("option", 
                new Grit.Sequence.Repository.MySql.SqlOption { 
                    ConnectionString = ConfigurationManager.ConnectionStrings["Sequence"].ConnectionString });
            Container.Bind<ISequenceService>().To<SequenceService>().InSingletonScope();

            Yue.Common.Repository.SqlOption sqlOptionBooking = 
                new Common.Repository.SqlOption { 
                    ConnectionString = ConfigurationManager.ConnectionStrings["Bookings"].ConnectionString };

            Container.Bind<IBookingRepository>().To<BookingRepository>().InSingletonScope()
                .WithConstructorArgument("option", sqlOptionBooking);
            Container.Bind<IBookingService>().To<BookingService>().InSingletonScope();

            Yue.Common.Repository.SqlOption sqlOptionUser =
                new Common.Repository.SqlOption
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Users"].ConnectionString
                };

            Container.Bind<IUserRepository>().To<UserRepository>().InSingletonScope()
                .WithConstructorArgument("option", sqlOptionUser);
            Container.Bind<IUserService>().To<UserService>().InSingletonScope();

            Container.Bind<IUserSecurityRepository>().To<UserSecurityRepository>().InSingletonScope()
                .WithConstructorArgument("option", sqlOptionUser);
            Container.Bind<IUserSecurityService>().To<UserSecurityService>().InSingletonScope();
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