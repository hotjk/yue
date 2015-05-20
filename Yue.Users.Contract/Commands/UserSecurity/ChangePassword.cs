using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class ChangePassword : PasswordCommandBase
    {
        public ChangePassword(int userId, string passwordHash,
            DateTime createAt, int createBy)
            : base(userId, UserSecurityCommand.ChangePassword, passwordHash, createAt, createBy)
        {
        }
    }
}
