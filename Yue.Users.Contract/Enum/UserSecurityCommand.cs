using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract
{
    public enum UserSecurityCommand
    {
        CreateUserSecurity = 0,
        RequestActivateToken = 1,
        ActivateUser = 2,
        VerifyPassword = 3,
        RequestResetPasswordToken = 10,
        ResetPassword = 11,
        ChangePassword = 12,
        Block = 20,
        Destory = 21,
        Restore = 22,
    }
}
