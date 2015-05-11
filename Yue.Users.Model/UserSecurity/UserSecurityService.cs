using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract.Events;

namespace Yue.Users.Model
{
    public class UserSecurityService : IUserSecurityService
    {
        private IUserSecurityRepository _repository;

        public UserSecurityService(IUserSecurityRepository repository)
        {
            this._repository = repository;
        }

        public UserSecurity Get(int userId)
        {
            return _repository.Get(userId);
        }
    }
}
