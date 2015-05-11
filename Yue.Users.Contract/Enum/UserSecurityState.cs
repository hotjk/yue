using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract
{
    public enum UserSecurityState
    {
        Initial = 0,
        Normal = 1,
        Blocked = 10,
        Destroyed = 11
    }
}
