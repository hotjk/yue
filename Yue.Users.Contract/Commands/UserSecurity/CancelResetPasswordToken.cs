using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class CancelResetPasswordToken : TokenCommandBase
    {
        public CancelResetPasswordToken(int userId, DateTime createAt, int createBy, string token)
            : base(userId, UserSecurityCommand.CancelResetPasswordToken, token, createAt, createBy)
        {
        }
    }
}
