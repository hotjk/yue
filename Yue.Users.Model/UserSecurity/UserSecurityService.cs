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

        public string PasswordHash(int userId, string password)
        {
            return Grit.Utility.Security.PasswordHash.CreateHash(string.Format("{0} {1}", userId, password));
        }

        public bool VerifyPassword(int userId, string password)
        {
            var userSecurity = _repository.Get(userId);
            bool match = Grit.Utility.Security.PasswordHash.ValidatePassword(string.Format("{0} {1}", userId, password), userSecurity.PasswordHash);
            return match;
        }

        public UserSecurity Get(int userId)
        {
            return _repository.Get(userId);
        }

        public UserSecurity UserSecurityByEmail(string email)
        {
            return _repository.UserSecurityByEmail(email);
        }
    }
}
