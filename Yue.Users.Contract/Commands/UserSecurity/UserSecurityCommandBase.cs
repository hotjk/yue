using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class UserSecurityCommandBase
    {
        public UserSecurityCommandBase(int userId, UserSecurityCommand type, 
            string passwordHash,
            DateTime createAt, int createBy)
        {
            this.UserId = userId;
            this.Type = type;
            this.PasswordHash = passwordHash;
            this.CreateAt = createAt;
            this.CreateBy = createBy;
        }

        public int UserId { get; private set; }
        public UserSecurityCommand Type { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
