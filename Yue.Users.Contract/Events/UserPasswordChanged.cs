using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Events
{
    public class UserPasswordChanged : ACE.IEvent
    {
        public UserPasswordChanged(int userId, DateTime changeAt, int changeBy)
        {
            this.UserId = userId;
            this.CreateAt = changeAt;
            this.CreateBy = changeBy;
        }
        public int UserId { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
