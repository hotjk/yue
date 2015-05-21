using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public abstract class UserActionBase : ACE.Action
    {
        public UserActionBase(int userId, DateTime createAt, int createBy)
        {
            this.UserId = userId;
            this.CreateAt = createAt;
            this.CreateBy = createBy;
        }
        public int UserId { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
