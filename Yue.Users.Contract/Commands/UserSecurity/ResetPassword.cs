using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class ResetPassword : PasswordCommandBase
    {
        public ResetPassword(int userId, string passwordHash,
            DateTime createAt, int createBy)
            : base(userId, UserSecurityCommand.ResetPassword, passwordHash, createAt, createBy)
        {
        }
    }
}
