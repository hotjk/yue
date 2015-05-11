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
        Activate = 1,
        ChangePassword = 11,
        ResetPassword = 12,
        VerifyUserPassword = 13,
        Block = 20,
        Destory = 21,
        Restore = 22,
    }
}
