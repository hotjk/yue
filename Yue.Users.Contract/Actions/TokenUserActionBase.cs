using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public abstract class TokenUserActionBase:UserActionBase
    {
        public TokenUserActionBase(int userId, DateTime createAt, int createBy, string token)
            :base(userId, createAt, createBy)
        {
            this.Token = token;
        }
        public string Token { get; private set; }
    }
}
