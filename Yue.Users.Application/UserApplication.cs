﻿using ACE;
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

        public void Invoke(Yue.Users.Contract.Actions.Activate action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.Activate cmd = new Contract.Commands.Activate
                (action.UserId, action.Token, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.RequestResetPasswordToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.RequestResetPasswordToken cmd = new Contract.Commands.RequestResetPasswordToken
                (action.UserId, action.Token, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.CancelResetPasswordToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.CancelResetPasswordToken cmd = new Contract.Commands.CancelResetPasswordToken
                (action.UserId, action.CreateAt, action.CreateBy, action.Token);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.ResetPassword action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.ResetPassword cmd = new Contract.Commands.ResetPassword
                (action.UserId, action.PasswordHash, action.Token, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }

        public void Invoke(Contract.Actions.VerifyResetPasswordToken action)
        {
            using (UnitOfWork unitOfwork = new UnitOfWork(_eventBus))
            {
                Yue.Users.Contract.Commands.VerifyResetPasswordToken cmd = new Contract.Commands.VerifyResetPasswordToken
                (action.UserId, action.Token, action.CreateAt, action.CreateBy);
                _commandBus.Send(cmd);

                unitOfwork.Complete();
            }
        }
    }
}
