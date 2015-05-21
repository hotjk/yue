using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class ResetPassword : PasswordCommandBase
    {
        public ResetPassword(int userId, string passwordHash,string token,
            DateTime createAt, int createBy)
            : base(userId, UserSecurityCommand.ResetPassword, passwordHash, createAt, createBy)
        {
            this.Token = token;
        }
        public string Token { get; private set; }
    }
}
