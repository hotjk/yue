using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract.Commands;
using Yue.Users.Model;

namespace Yue.Users.Handler
{
    public interface IUserSecurityWriteRepository
    {
        UserSecurity Get(int userId);
        UserSecurity GetForUpdate(int userId);
        bool Add(UserSecurity userSecurity);
        bool Update(UserSecurity userSecurity);
        bool Log(UserSecurityCommandBase command);
    }
}
