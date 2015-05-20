using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public abstract class PasswordCommandBase : UserSecurityCommandBase, ACE.ICommand
    {
        public PasswordCommandBase(int userId, UserSecurityCommand command, string passwordHash,
            DateTime createAt, int createBy)
            : base(userId, command, createAt, createBy)
        {
            this.PasswordHash = passwordHash;
        }
        public string PasswordHash { get; private set; }
    }
}
