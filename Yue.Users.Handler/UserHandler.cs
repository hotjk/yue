using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Users.Contract.Commands;
using Yue.Users.Model;

namespace Yue.Users.Handler
{
    public class UserHandler :
        ICommandHandler<CreateUser>,
        ICommandHandler<ActivateUser>
    {
        private IEventBus _eventBus;
        private IUserWriteRepository _repository;

        public UserHandler(IEventBus eventBus, 
            IUserWriteRepository repository)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public void Execute(CreateUser command)
        {
            User found = _repository.UserByEmail(command.Email);
            if (found != null)
            {
                throw new BusinessException(BusinessStatusCode.Conflict, "Email already existed.");
            }
            User user = User.Create(command);
            _repository.Add(user);
        }

        public void Execute(ActivateUser command)
        {
            User user = _repository.GetForUpdate(command.UserId);
            if (user == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "User not found.");
            }
            user.EnsoureAndUpdateState(command);
            _repository.Update(user);
        }
    }
}
