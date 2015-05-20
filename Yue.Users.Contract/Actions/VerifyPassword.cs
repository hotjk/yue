using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class VerifyPassword : UserActionBase
    {
        public VerifyPassword(int userId, string passwordHash, DateTime createAt, int createBy)
        {
            this.UserId = userId;
            this.PasswordHash = passwordHash;
            this.CreateAt = createAt;
            this.CreateBy = createBy;
        }

        public int UserId { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreateAt { get; set; }
        public int CreateBy {get;set;}
    }
}
