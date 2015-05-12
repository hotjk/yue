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
            _repository.Log(command);
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

            UserPasswordChanged evt = new UserPasswordChanged(userSecurity.UserId, command.CreateAt, command.CreateBy);
            _eventBus.Publish(evt.ToExternalQueue());
        }

        public void Execute(ResetPassword command)
        {
            UserSecurity userSecurity = _repository.GetForUpdate(command.UserId);
            if (userSecurity == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "User not found.");
            }
            
            userSecurity.ChangePassword(command);
            _repository.Update(userSecurity);
            _repository.Log(command);

            UserPasswordChanged evt = new UserPasswordChanged(userSecurity.UserId, command.CreateAt, command.CreateBy);
            _eventBus.Publish(evt.ToExternalQueue());
        }
    }
}
