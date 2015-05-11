using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Model
{
    public interface IUserRepository
    {
        User Get(int userId);
        IEnumerable<User> Users(IEnumerable<int> ids);
    }
}
