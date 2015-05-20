using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Users.Contract.Actions;
using Yue.Users.Contract.Commands;
using Yue.Users.Model;

namespace Yue.Users.Application
{
    public class UserApplication :
        IActionHandler<Contract.Actions.Register>,
        IActionHandler<Contract.Actions.VerifyPassword>,
        IActionHandler<Contract.Actions.ChangePassword>,
        IActionHandler<Contract.Actions.ResetPassword>,
        IActionHandler<Contract.Actions.RequestActivateToken>,
        IActionHandler<Contract.Actions.Activate>
    {
        protected ICommandBus _commandBus;
        protected IEventBus _eventBus;
        private IUserService _userService;

        public UserApplication(ICommandBus commandBus, IEventBus eventBus,
            IUserService userService)
        {
            this._commandBus = commandBus;
            this._eventBus = eventBus;
            _userService = userService;
        }

        public void Invoke(Register action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                CreateUser createUser = new CreateUser(
                    action.UserId, 
                    action.Email,
                    action.Name, 
                    action.CreateAt,
                    action.UserId);
                _commandBus.Send(createUser);

                CreateUserSecurity createUserSecurity = new CreateUserSecurity(
                    action.UserId,
                    action.PasswordHash,
                    action.CreateAt,
                    action.UserId);
                _commandBus.Send(createUserSecurity);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.ChangePassword action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.ChangePassword cmd = new Contract.Commands.ChangePassword
                (action.UserId, action.PasswordHash, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.ResetPassword action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.ResetPassword cmd = new Contract.Commands.ResetPassword
                (action.UserId, action.PasswordHash, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.RequestActivateToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.RequestActivateToken cmd = new Contract.Commands.RequestActivateToken
                (action.UserId, action.Token, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Activate action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.ActivateUser cmd = new Contract.Commands.ActivateUser
                (action.UserId, action.Token, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.VerifyPassword action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.VerifyPassword cmd = new Contract.Commands.VerifyPassword
                (action.UserId, action.PasswordHash, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }
    }
}
