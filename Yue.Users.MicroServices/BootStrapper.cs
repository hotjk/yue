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
using Yue.Users.Handler;
using Yue.Users.Repository.Write;
using Yue.Common.Log;
using Yue.Users.Repository;
using Yue.Users.Model;
using Autofac;

namespace Yue.Users.MicroServices
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
                .WithParameter(Constants.ParamCommandAssmblies, new string[] { "Yue.Users.ContractFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "Yue.Users.Handler" });
            _builder.RegisterType<CommandBus>().As<ICommandBus>().SingleInstance();

            _builder.RegisterType<EventHandlerFactory>().As<IEventHandlerFactory>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container))
                .WithParameter(Constants.ParamEventAssmblies, new string[] { "Yue.Users.ContractFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "Yue.Users.Handler" });
            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            _builder.RegisterType<EventBus>().As<IEventBus>().InstancePerLifetimeScope();

            _builder.RegisterType<ActionHandlerFactory>().As<IActionHandlerFactory>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container))
                .WithParameter(Constants.ParamActionAssmblies, new string[] { "Yue.Users.ContractFS" })
                .WithParameter(Constants.ParamHandlerAssmblies, new string[] { "Yue.Users.Application" });
            _builder.RegisterType<ActionStation>().As<IActionStation>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Autofac.IContainer), Container));
        }

        private static void BindBusinessObjects()
        {
            Yue.Common.Repository.SqlOption sqlOptionUserWrite =
                new Common.Repository.SqlOption
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Users.Write"].ConnectionString
                };

            _builder.RegisterType<UserWriteRepository>().As<IUserWriteRepository>().SingleInstance()
                .WithParameter("option", sqlOptionUserWrite);
            _builder.RegisterType<UserSecurityWriteRepository>().As<IUserSecurityWriteRepository>().SingleInstance()
                .WithParameter("option", sqlOptionUserWrite);

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
