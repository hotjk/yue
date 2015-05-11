using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class CreateUserSecurity : UserSecurityCommandBase, ACE.ICommand
    {
        public CreateUserSecurity(int userId,
            string passwordHash,
            DateTime createAt,
            int createBy) : base(userId, UserSecurityCommand.Create, passwordHash, createAt, createBy)
        {
        }
    }
}
