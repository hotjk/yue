using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract;

namespace Yue.Users.Model.Commands
{
    public abstract class UserSecurityCommandBase
    {
        public UserSecurityCommandBase(int userId, UserSecurityCommand type, 
            DateTime createAt, int createBy)
        {
            this.UserId = userId;
            this.Type = type;
            this.CreateAt = createAt;
            this.CreateBy = createBy;
        }

        public int UserId { get; private set; }
        public UserSecurityCommand Type { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
