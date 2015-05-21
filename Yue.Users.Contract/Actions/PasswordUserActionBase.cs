using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public abstract class PasswordUserActionBase:UserActionBase
    {
        public PasswordUserActionBase(int userId, DateTime createAt, int createBy, string passwordHash)
            :base(userId, createAt, createBy)
        {
            this.PasswordHash = passwordHash;
        }
        public string PasswordHash { get; private set; }
    }
}
