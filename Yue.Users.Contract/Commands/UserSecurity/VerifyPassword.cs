using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class VerifyPassword : UserSecurityCommandBase, ACE.ICommand
    {
        public VerifyPassword(int userId, string passwordHash,
            DateTime createAt, int createBy)
            : base(userId, UserSecurityCommand.VerifyPassword, passwordHash, createAt, createBy)
        {
        }
    }
}
