using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class UserCommandBase
    {
        public UserCommandBase(UserCommand type, DateTime createAt, int createBy)
        {
            this.Type = type;
            this.CreateAt = createAt;
            this.CreateBy = createBy;
        }

        public UserCommand Type { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
