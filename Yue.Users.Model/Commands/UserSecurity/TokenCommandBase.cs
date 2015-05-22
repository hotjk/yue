using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract;

namespace Yue.Users.Model.Commands
{
    public abstract class TokenCommandBase : UserSecurityCommandBase, ACE.ICommand
    {
        public TokenCommandBase(int userId, UserSecurityCommand command, string token, DateTime createAt, int createBy)
            : base(userId, command, createAt, createBy)
        {
            this.Token = token;
        }

        public string Token { get; protected set; }
    }
}
