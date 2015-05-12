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

        public string PasswordHash(string password)
        {
            return Grit.Utility.Security.PasswordHash.CreateHash(password);
        }

        public bool VerifyPassword(int userId, string password)
        {
            var userSecurity = _repository.Get(userId);
            bool match = Grit.Utility.Security.PasswordHash.ValidatePassword(password, userSecurity.PasswordHash);
            return match;
        }
    }
}
