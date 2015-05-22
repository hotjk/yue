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
            this.ChangeAt = changeAt;
            this.ChangeBy = changeBy;
        }
        public int UserId { get; private set; }
        public DateTime ChangeAt { get; private set; }
        public int ChangeBy { get; private set; }
    }
}
