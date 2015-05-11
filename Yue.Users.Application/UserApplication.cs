using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract.Actions;
using Yue.Users.Contract.Commands;

namespace Yue.Users.Application
{
    public class UserApplication : 
        IActionHandler<Register>
    {
        protected ICommandBus CommandBus { get; private set; }
        protected IEventBus EventBus { get; private set; }

        public UserApplication(ICommandBus commandBus, IEventBus eventBus)
        {
            this.CommandBus = commandBus;
            this.EventBus = eventBus;
        }

        public void Invoke(Register action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(EventBus))
            {
                CreateUser createUser = new CreateUser(
                    action.UserId, 
                    action.Email,
                    action.Name, 
                    action.RegisterAt,
                    action.UserId);
                CommandBus.Send(createUser);

                CreateUserSecurity createUserSecurity = new CreateUserSecurity(
                    action.UserId,
                    action.PasswordHash,
                    action.RegisterAt,
                    action.UserId);
                CommandBus.Send(createUserSecurity);

                unitOfwork.Complete();
            }
        }
    }
}
