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
        ChangePassword = 1,
        ResetPassword = 2,
        VerifyUserPassword = 3,
        Block = 10,
        Destory = 11,
        Restore = 20,
    }
}
