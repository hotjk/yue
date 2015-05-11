using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Model.Write
{
    public interface IUserSecurityWriteRepository
    {
        UserSecurity UserSecurityByEmail(string email);
        bool Add(UserSecurity userSecurity);
        bool Update(User user);
        bool AddLog(UserSecurity userSecurity);
    }
}
