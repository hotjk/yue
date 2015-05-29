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
        ResetActivateToken = 2,
        VerifyPassword = 3,
        RequestResetPasswordToken = 10,
        VerifyResetPasswordToken = 11,
        CancelResetPasswordToken = 12,
        ResetPassword = 13,
        ChangePassword = 14,
        Block = 20,
        Destory = 21,
        Restore = 22,
    }
}