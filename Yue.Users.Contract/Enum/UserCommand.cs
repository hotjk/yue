using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract
{
    public enum UserCommand
    {
        Create = 0,
        Activate = 1,
        ChangeProfile = 2,
        Block = 20,
        Destory = 21,
        Restore = 22,
    }
}
