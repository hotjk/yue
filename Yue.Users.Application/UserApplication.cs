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
        IActionHandler<Register>,
        IActionHandler<Login>,
        IActionHandler<Yue.Users.Contract.Actions.ChangePassword>,
        IActionHandler<Yue.Users.Contract.Actions.ResetPassword>
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

        public void Invoke(Login action)
        {
            User user = _userService.UserByEmail(action.Email);
            if(user == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "User not found.");
            }

            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                VerifyPassword verifyUserPassword = new VerifyPassword(
                    user.UserId,
                    action.PasswordHash,
                    action.CreateAt,
                    user.UserId);
                _commandBus.Send(verifyUserPassword);

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
            }
        }

        public void Invoke(Contract.Actions.ResetPassword action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.ResetPassword cmd = new Contract.Commands.ResetPassword
                (action.UserId, action.PasswordHash, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);
            }
        }
    }
}
