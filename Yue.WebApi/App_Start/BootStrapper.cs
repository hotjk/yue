using ACE;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Grit.Utility.Authentication;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Yue.Bookings.Model;
using Yue.Bookings.Repository;
using Yue.Users.Model;
using Yue.Users.Repository;
using Autofac;
using EasyNetQ.Loggers;

namespace Yue.WebApi
{
    public class BootStrapper
    {
        public static Autofac.IContainer Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }

        private static ContainerBuilder _builder;

        public static void BootStrap()
        {
            var adapter = new EasyNetQ.DI.AutofacAdapter(new ContainerBuilder());
            Container = adapter.Container;

            EasyNetQ.RabbitHutch.SetContainerFactory(() => { return adapter; });
            EasyNetQBus = EasyNetQ.RabbitHutch.CreateBus(ConfigurationManager.ConnectionStrings["RabbitMq"].ConnectionString,
                x => x.Register<EasyNetQ.IEasyNetQLogger, NullLogger>());

            _builder = new ContainerBuilder();
            BindFrameworkObjects();
            BindBusinessObjects();
            _builder.Update(Container);
        }

        private static void BindFrameworkObjects()
        {
            _builder.RegisterType<Log4NetBusLogger>().As<ACE.Loggers.IBusLogger>().SingleInstance();

            _builder.RegisterType<EventBus>().As<IEventBus>().InstancePerLifetimeScope();
            _builder.RegisterType<ActionBus>().As<IActionBus>().InstancePerLifetimeScope();
            _builder.RegisterInstance<CookieTicketConfig>(CookieTicketConfig.Default()).As<ICookieTicketConfig>().SingleInstance();
            _builder.RegisterType<Authenticator>().As<IAuthenticator>().SingleInstance();

            //_builder.RegisterType<MockAuthenticator>().As<IAuthenticator>().SingleInstance().WithParameter("userId", 33);
        }

        private static void BindBusinessObjects()
        {
            _builder.RegisterType<SequenceRepository>().As<ISequenceRepository>().SingleInstance()
                .WithParameter("option", new Grit.Sequence.Repository.MySql.SqlOption { 
                    ConnectionString = ConfigurationManager.ConnectionStrings["Sequence"].ConnectionString });
            _builder.RegisterType<SequenceService>().As<ISequenceService>().SingleInstance();


            Yue.Common.Repository.SqlOption sqlOptionBooking =
                new Common.Repository.SqlOption
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Bookings"].ConnectionString
                };

            _builder.RegisterType<BookingRepository>().As<IBookingRepository>().SingleInstance()
                .WithParameter("option", sqlOptionBooking);
            _builder.RegisterType<BookingService>().As<IBookingService>().SingleInstance();
            
            Yue.Common.Repository.SqlOption sqlOptionUser =
                new Common.Repository.SqlOption
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Users"].ConnectionString
                };

            _builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance()
                .WithParameter("option", sqlOptionUser);
            _builder.RegisterType<UserService>().As<IUserService>().SingleInstance();

            _builder.RegisterType<UserSecurityRepository>().As<IUserSecurityRepository>().SingleInstance()
               .WithParameter("option", sqlOptionUser);
            _builder.RegisterType<UserSecurityService>().As<IUserSecurityService>().SingleInstance();
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