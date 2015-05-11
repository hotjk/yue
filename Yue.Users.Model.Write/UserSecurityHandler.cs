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
        ICommandHandler<VerifyUserPassword>
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

        public void Execute(VerifyUserPassword command)
        {
            UserSecurity userSecurity = _repository.UserSecurityByEmail(command.Email);
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
    }
}
