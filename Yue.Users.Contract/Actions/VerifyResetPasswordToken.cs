using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class VerifyResetPasswordToken : TokenUserActionBase
    {
        public VerifyResetPasswordToken(int userId, DateTime createAt, int createBy, string token)
            :base(userId, createAt, createBy, token)
        {
        }
    }
}
