using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract
{
    public enum UserSecurityCommand
    {
        Create = 0,
        ChangePassword = 11,
        ResetPassword = 12,
    }
}
