using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract;

namespace Yue.Users.Model.Commands
{
    public class CreateUserSecurity : UserSecurityCommandBase, ACE.ICommand
    {
        public CreateUserSecurity(int userId,
            string passwordHash,
            DateTime createAt,
            int createBy) : base(userId, UserSecurityCommand.CreateUserSecurity, createAt, createBy)
        {
            this.PasswordHash = passwordHash;
        }
        public string PasswordHash { get; private set; }
    }
}
