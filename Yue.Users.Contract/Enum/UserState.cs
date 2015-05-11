using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract
{
    public enum UserState
    {
        Initial = 0,
        Inactive = 1,
        Normal = 2,
        Blocked = 10,
        Destroyed = 11
    }
}
