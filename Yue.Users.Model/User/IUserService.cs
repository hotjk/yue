using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Model
{
    public interface IUserService
    {
        User Get(int userId);
        User UserByEmail(string email);
        IEnumerable<User> Users(IEnumerable<int> ids);
    }
}
