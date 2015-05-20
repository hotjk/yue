using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Events
{
    public class UserActiviteTokenGenerated : ACE.Event
    {
        public UserActiviteTokenGenerated(int userId, string token, DateTime changeAt, int changeBy)
        {
            this.UserId = userId;
            this.Token = token;
            this.ChangeAt = changeAt;
            this.ChangeBy = changeBy;
        }
        public int UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime ChangeAt { get; private set; }
        public int ChangeBy { get; private set; }
    }
}
