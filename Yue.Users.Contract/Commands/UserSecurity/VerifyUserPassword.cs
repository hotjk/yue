using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class VerifyUserPassword : UserSecurityCommandBase, ACE.ICommand
    {
        public VerifyUserPassword(string email, string passwordHash,
            DateTime createAt, int createBy) : base(UserSecurityCommand.VerifyUserPassword, createAt, createBy)
        {
            this.Email = email;
            this.PasswordHash = passwordHash;
        }

        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
    }
}
