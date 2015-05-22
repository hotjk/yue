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
using Yue.Users.Model.Write;
using Yue.Users.Repository.Write;
using Yue.Common.Log;
using Yue.Users.Repository;
using Yue.Users.Model;

namespace Yue.Users.MicroServices
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
                .WithConstructorArgument(Constants.ParamCommandAssmblies, new string[] { "Yue.Users.Contract" })
                .WithConstructorArgument(Constants.ParamHandlerAssmblies, new string[] { "Yue.Users.Model.Write" });
            Container.Bind<ICommandBus>().To<CommandBus>().InSingletonScope();
            Container.Bind<IEventHandlerFactory>().To<EventHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument(Constants.ParamEventAssmblies, new string[] { "Yue.Users.Contract" })
                .WithConstructorArgument(Constants.ParamHandlerAssmblies, new string[] { "Yue.Users.Model.Write" });
            // EventBus must be thread scope, published events will be saved in thread EventBus._events, until Flush/Clear.
            Container.Bind<IEventBus>().To<EventBus>().InThreadScope();
            Container.Bind<IActionHandlerFactory>().To<ActionHandlerFactory>()
                .InSingletonScope()
                .WithConstructorArgument(Constants.ParamActionAssmblies, new string[] { "Yue.Users.Contract" })
                .WithConstructorArgument(Constants.ParamHandlerAssmblies, new string[] { "Yue.Users.Application" });
            // ActionBus must be thread scope, single thread bind to use single anonymous RabbitMQ queue for reply.
            Container.Bind<IActionBus>().To<ActionBus>().InThreadScope();
        }

        private static void BindBusinessObjects()
        {
            Yue.Common.Repository.SqlOption sqlOptionWrite =
                new Common.Repository.SqlOption
                {
                    ConnectionString = ConfigurationManager.ConnectionStrings["Users.Write"].ConnectionString
                };

            Container.Bind<IUserWriteRepository>().To<UserWriteRepository>().InSingletonScope()
                .WithConstructorArgument("option", sqlOptionWrite);
            Container.Bind<IUserSecurityWriteRepository>().To<UserSecurityWriteRepository>().InSingletonScope()
                .WithConstructorArgument("option", sqlOptionWrite);

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
