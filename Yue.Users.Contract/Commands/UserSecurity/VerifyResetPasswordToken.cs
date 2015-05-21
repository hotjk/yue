using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class VerifyResetPasswordToken : TokenCommandBase
    {
        public VerifyResetPasswordToken(int userId, string token, DateTime createAt, int createBy)
            : base(userId, UserSecurityCommand.VerifyResetPasswordToken, token, createAt, createBy)
        {
        }
    }
}
