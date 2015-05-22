using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Events
{
    public class UserActiviteTokenGenerated : ACE.IEvent
    {
        public UserActiviteTokenGenerated(int userId, string token, DateTime changeAt, int changeBy)
        {
            this.UserId = userId;
            this.Token = token;
            this.CreateAt = changeAt;
            this.CreateBy = changeBy;
        }
        public int UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
