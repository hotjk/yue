using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Users.Contract.Commands;
using Yue.Users.Contract.Events;

namespace Yue.Users.Model.Write
{
    public class UserSecurityHandler :
        ICommandHandler<CreateUserSecurity>,
        ICommandHandler<VerifyPassword>,
        ICommandHandler<ChangePassword>,
        ICommandHandler<ResetPassword>
    {
        private IEventBus _eventBus;
        private IUserSecurityWriteRepository _repository;

        public UserSecurityHandler(IEventBus eventBus,
            IUserSecurityWriteRepository repository)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public void Execute(CreateUserSecurity command)
        {
            UserSecurity userSecurity = UserSecurity.Create(command);
            _repository.Add(userSecurity);
        }

        public void Execute(VerifyPassword command)
        {
            UserSecurity userSecurity = _repository.Get(command.UserId);
            bool match = false;
            if (userSecurity != null)
            {
                if (userSecurity.PasswordMatchWith(command.PasswordHash))
                {
                    match = true;
                }
                UserPasswordVerified evt = new UserPasswordVerified(userSecurity.UserId, match, command.CreateAt, command.CreateBy);
                _eventBus.Publish(evt.ToExternalQueue());
            }
            
            if (!match)
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid user password.");
            }
        }

        public void Execute(ChangePassword command)
        {
            UserSecurity userSecurity = _repository.GetForUpdate(command.UserId);
            if (userSecurity == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "User not found.");
            }
            userSecurity.ChangePassword(command);
            _repository.Update(userSecurity);
            _repository.Log(command);
        }

        public void Execute(ResetPassword command)
        {
            throw new NotImplementedException();
        }
    }
}
