using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Events
{
    public class UserPasswordVerified : ACE.IEvent
    {
        public UserPasswordVerified(int userId, bool match, DateTime verifyAt, int verifyBy)
        {
            this.UserId = userId;
            this.Match = match;
            this.CreateAt = verifyAt;
            this.CreateBy = verifyBy;
        }
        public int UserId { get; private set; }
        public bool Match { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
