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
        IActionHandler<Contract.Actions.ChangePassword>,
        IActionHandler<Contract.Actions.RequestActivateToken>,
        IActionHandler<Contract.Actions.Activate>,
        IActionHandler<Contract.Actions.RequestResetPasswordToken>,
        IActionHandler<Contract.Actions.VerifyResetPasswordToken>,
        IActionHandler<Contract.Actions.ResetPassword>,
        IActionHandler<Contract.Actions.CancelResetPasswordToken>
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
                _commandBus.Send(new CreateUser(
                    action.UserId,
                    action.CreateAt,
                    action.CreateBy,
                    action.Email,
                    action.Name));

                _commandBus.Send(new CreateUserSecurity(
                    action.UserId,
                    action.CreateAt,
                    action.CreateBy,
                    action.PasswordHash));

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.ChangePassword action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                _commandBus.Send(new Yue.Users.Contract.Commands.ChangePassword
                (action.UserId, action.CreateAt, action.CreateBy, action.PasswordHash));

                unitOfwork.Complete();
            }
        }

        

        public void Invoke(Contract.Actions.RequestActivateToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                _commandBus.Send(new Yue.Users.Contract.Commands.RequestActivateToken
                (action.UserId, action.CreateAt, action.CreateBy, action.Token));

                unitOfwork.Complete();
            }
        }

        public void Invoke(Yue.Users.Contract.Actions.Activate action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                _commandBus.Send(new Yue.Users.Contract.Commands.ResetActivateToken
                (action.UserId, action.CreateAt, action.CreateBy, action.Token));

                _commandBus.Send(new Yue.Users.Contract.Commands.ActivateUser
                (action.UserId, action.CreateAt, action.CreateBy));

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.RequestResetPasswordToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                _commandBus.Send(new Yue.Users.Contract.Commands.RequestResetPasswordToken
                (action.UserId, action.CreateAt, action.CreateBy, action.Token));

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.CancelResetPasswordToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                _commandBus.Send(new Yue.Users.Contract.Commands.CancelResetPasswordToken
                (action.UserId, action.CreateAt, action.CreateBy, action.Token));

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.ResetPassword action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                _commandBus.Send(new Yue.Users.Contract.Commands.ResetPassword
                (action.UserId, action.CreateAt, action.CreateBy, action.Token, action.PasswordHash));

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.VerifyResetPasswordToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                _commandBus.Send(new Yue.Users.Contract.Commands.VerifyResetPasswordToken
                (action.UserId, action.CreateAt, action.CreateBy, action.Token));

                unitOfwork.Complete();
            }
        }
    }
}
