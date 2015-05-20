using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class RequestResetPasswordToken : TokenCommandBase
    {
        public RequestResetPasswordToken(int userId, string token, DateTime createAt, int createBy)
            : base(userId, UserSecurityCommand.RequestResetPasswordToken, token, createAt, createBy)
        {
        }
    }
}
