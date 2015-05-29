using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Model
{
    public interface IUserSecurityRepository
    {
        UserSecurity Get(int userId);
        UserSecurity UserSecurityByEmail(string email);
    }
}
