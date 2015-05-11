using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class UserSecurityCommandBase
    {
        public UserSecurityCommandBase(UserSecurityCommand type, DateTime createAt, int createBy)
        {
            this.Type = type;
            this.CreateAt = createAt;
            this.CreateBy = createBy;
        }

        public UserSecurityCommand Type { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
