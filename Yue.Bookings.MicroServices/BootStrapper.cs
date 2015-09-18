using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using EasyNetQ.Loggers;
using Grit.Sequence;
using Grit.Sequence.Repository.MySql;
using Yue.Bookings.Repository;
using Yue.Bookings.Handler;
using Yue.Bookings.Repository.Write;
using Yue.Common.Log;
using Autofac;

namespace Yue.Bookings.MicroServices
{
    public class BootStrapper
    {
        public static Autofac.IContainer Container { get; private set; }
        public static EasyNetQ.IBus EasyNetQBus { get; private set; }
        public static IActionStation ActionStation { get; private set; }

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

            ActionStation = Container.Resolve<IActionStation>();
        }

        private static void BindFrameworkObjects()
        {
            _builder.RegisterType<Log4NetBusLogger>().As<ACE.Loggers.IBusLogger>().SingleInstance();

            _builder.RegisterType<CommandHandlerFactory>().As<ICommandHandlerFactory>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container))
                .WithParameter(Constants.ParamCommandAssmblies, new string[] { "Yue.Bookings.ContractFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "Yue.Bookings.Handler" });
            _builder.RegisterType<CommandBus>().As<ICommandBus>().SingleInstance();

            _builder.RegisterType<EventHandlerFactory>().As<IEventHandlerFactory>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container))
                .WithParameter(Constants.ParamEventAssmblies, new string[] { "Yue.Bookings.ContractFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "Yue.Bookings.Handler" });
            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            _builder.RegisterType<EventBus>().As<IEventBus>().InstancePerLifetimeScope();

            _builder.RegisterType<ActionHandlerFactory>().As<IActionHandlerFactory>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container))
                .WithParameter(Constants.ParamActionAssmblies, new string[] { "Yue.Bookings.ContractFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "Yue.Bookings.Application" });
            _builder.RegisterType<ActionStation>().As<IActionStation>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container));
        }

        private static void BindBusinessObjects()
        {
            Yue.Common.Repository.SqlOption sqlOptionWrite =
                new Common.Repository.SqlOption
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Bookings.Write"].ConnectionString
                };

            _builder.RegisterType<BookingWriteRepository>().As<IBookingWriteRepository>().SingleInstance()
               .WithParameter("option", sqlOptionWrite);
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
