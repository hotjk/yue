using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Users.Model.Commands;
using Yue.Users.Contract.Events;

namespace Yue.Users.Model.Write
{
    public class UserSecurityHandler :
        ICommandHandler<CreateUserSecurity>,
        ICommandHandler<ChangePassword>,
        ICommandHandler<RequestActivateToken>,
        ICommandHandler<Activate>,
        ICommandHandler<RequestResetPasswordToken>,
        ICommandHandler<VerifyResetPasswordToken>,
        ICommandHandler<CancelResetPasswordToken>,
        ICommandHandler<ResetPassword>
    {
        private IEventBus _eventBus;
        private IUserWriteRepository _userRepository;
        private IUserSecurityWriteRepository _userSecurityRepository;

        public UserSecurityHandler(IEventBus eventBus,
            IUserWriteRepository userRepository,
            IUserSecurityWriteRepository userSecurityRepository)
        {
            _eventBus = eventBus;
            _userRepository = userRepository;
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
            User user = _userRepository.GetForUpdate(command.UserId);
            user.EnsoureAndUpdateState(command);

            UserSecurity userSecurity = _userSecurityRepository.Get(command.UserId);
            userSecurity.UpdatePassword(command);

            _userRepository.Update(user);
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);

            UserPasswordChanged evt = new UserPasswordChanged(userSecurity.UserId, command.CreateAt, command.CreateBy);
            _eventBus.Publish(evt);
        }

        public void Execute(RequestActivateToken command)
        {
            User user = _userRepository.GetForUpdate(command.UserId);
            user.EnsoureAndUpdateState(command);

            UserSecurity userSecurity = _userSecurityRepository.Get(command.UserId);
            userSecurity.UpdateActivateToken(command);

            _userRepository.Update(user);
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(Activate command)
        {
            User user = _userRepository.GetForUpdate(command.UserId);
            user.EnsoureAndUpdateState(command);

            UserSecurity userSecurity = _userSecurityRepository.Get(command.UserId);
            if (!userSecurity.VerifyActivateToken(command.Token))
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid activate token");
            }

            userSecurity.ClearActivateToken(command);
            _userRepository.Update(user);
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(RequestResetPasswordToken command)
        {
            User user = _userRepository.GetForUpdate(command.UserId);
            user.EnsoureAndUpdateState(command);

            UserSecurity userSecurity = _userSecurityRepository.Get(command.UserId);
            userSecurity.UpdateResetPasswordToken(command);

            _userRepository.Update(user);
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(VerifyResetPasswordToken command)
        {
            User user = _userRepository.GetForUpdate(command.UserId);
            user.EnsoureAndUpdateState(command);

            UserSecurity userSecurity = _userSecurityRepository.Get(command.UserId);
            if(string.Compare(userSecurity.ResetPasswordToken, command.Token) != 0)
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid reset password token");
            }
        }

        public void Execute(ResetPassword command)
        {
            User user = _userRepository.GetForUpdate(command.UserId);
            user.EnsoureAndUpdateState(command);

            UserSecurity userSecurity = _userSecurityRepository.Get(command.UserId);
            if (!userSecurity.VerifyResetPasswordToken(command.Token))
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid reset password token");
            }
            userSecurity.ClearResetPasswordToken(command);
            userSecurity.UpdatePassword(command);
            
            _userRepository.Update(user);
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }

        public void Execute(CancelResetPasswordToken command)
        {
            User user = _userRepository.GetForUpdate(command.UserId);
            user.EnsoureAndUpdateState(command);

            UserSecurity userSecurity = _userSecurityRepository.Get(command.UserId);
            if (!userSecurity.VerifyResetPasswordToken(command.Token))
            {
                throw new BusinessException(BusinessStatusCode.Unauthorized, "Invalid reset password token");
            }
            userSecurity.ClearResetPasswordToken(command);
            _userRepository.Update(user);
            _userSecurityRepository.Update(userSecurity);
            _userSecurityRepository.Log(command);
        }
    }
}
