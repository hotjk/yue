using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Model
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            this._repository = repository;
        }

        public User Get(int userId)
        {
            return _repository.Get(userId);
        }

        public IEnumerable<User> Users(IEnumerable<int> ids)
        {
            return _repository.Users(ids);
        }
    }
}
