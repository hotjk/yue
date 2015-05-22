using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Model.Commands;

namespace Yue.Users.Model.Write
{
    public class UserHandler :
        ICommandHandler<CreateUser>
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
            User user = User.Create(command);
            _repository.Add(user);
        }
    }
}
