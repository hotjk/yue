using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Model
{
    public interface IUserSecurityService
    {
        string PasswordHash(int userId, string password);
        bool VerifyPassword(int userId, string password);
        UserSecurity Get(int userId);
        UserSecurity UserSecurityByEmail(string email);
    }
}
