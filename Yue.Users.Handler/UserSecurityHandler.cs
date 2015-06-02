using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Users.Contract;
using Yue.Users.Contract.Commands;
using Yue.Users.Contract.Events;
using Yue.Users.Model;

namespace Yue.Users.Handler
{
    public class UserSecurityHandler :
        ICommandHandler<CreateUserSecurity>,
        ICommandHandler<ChangePassword>,
        ICommandHandler<RequestActivateToken>,
        ICommandHandler<ResetActivateToken>,
        ICommandHandler<RequestResetPasswordToken>,
        ICommandHandler<VerifyResetPasswordToken>,
        ICommandHandler<CancelResetPasswordToken>,
        ICommandHandler<ResetPassword>
    {
        private IEventBus _eventBus;
        private IUserSecurityWriteRepository _userSecurityRepository;

        public UserSecurityHandler(IEventBus eventBus,
            IUserSecurityWriteRepository userSecurityRepository)
        {
            _eventBus = eventBus;
            _userSecurityRepository = userSecurityRepository;
        }

        public void Execute(CreateUserSecurity command)
        {
            UserSecurity userSecurity = UserSecurity.Create(command);
            _userSecurityRepository.Add(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(ChangePassword command)
        {
            UserSecurity userSecurity = _userSecurityRepository.GetForUpdate(command.UserId);
            userSecurity.EnsoureAndUpdateState(command);
            userSecurity.UpdatePassword(command);

            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);

            UserPasswordChanged evt = new UserPasswordChanged(userSecurity.UserId, command.CreateAt, command.CreateBy);
            _eventBus.Publish(evt);
        }

        public void Execute(RequestActivateToken command)
        {
            UserSecurity userSecurity = _userSecurityRepository.GetForUpdate(command.UserId);
            userSecurity.EnsoureAndUpdateState(command); 
            userSecurity.UpdateActivateToken(command);

            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(ResetActivateToken command)
        {
            UserSecurity userSecurity = _userSecurityRepository.GetForUpdate(command.UserId);
            userSecurity.EnsoureAndUpdateState(command);
            if (!userSecurity.VerifyActivateToken(command.Token))
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid activate token");
            }

            userSecurity.ClearActivateToken(command);
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(RequestResetPasswordToken command)
        {
            UserSecurity userSecurity = _userSecurityRepository.GetForUpdate(command.UserId);
            userSecurity.EnsoureAndUpdateState(command);
            userSecurity.UpdateResetPasswordToken(command);

            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(VerifyResetPasswordToken command)
        {
            UserSecurity userSecurity = _userSecurityRepository.GetForUpdate(command.UserId);
            userSecurity.EnsoureAndUpdateState(command); 
            if (string.Compare(userSecurity.ResetPasswordToken, command.Token) != 0)
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid reset password token");
            }
        }

        public void Execute(ResetPassword command)
        {
            UserSecurity userSecurity = _userSecurityRepository.GetForUpdate(command.UserId);
            userSecurity.EnsoureAndUpdateState(command);
            if (!userSecurity.VerifyResetPasswordToken(command.Token))
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid reset password token");
            }
            userSecurity.ClearResetPasswordToken(command);
            userSecurity.UpdatePassword(command);
            
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(CancelResetPasswordToken command)
        {
            UserSecurity userSecurity = _userSecurityRepository.GetForUpdate(command.UserId);
            userSecurity.EnsoureAndUpdateState(command);
            if (!userSecurity.VerifyResetPasswordToken(command.Token))
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid reset password token");
            }
            userSecurity.ClearResetPasswordToken(command);

            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }
    }
}
