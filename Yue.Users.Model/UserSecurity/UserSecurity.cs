using ACE.Exceptions;
using Grit.Pattern.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Users.Contract;
using Yue.Users.Contract.Commands;

namespace Yue.Users.Model
{
    public class UserSecurity : IAggregateRoot
    {
        public int UserId { get; private set; }
        public UserState State { get; private set; }
        private string PasswordHash { get; set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
        public DateTime UpdateAt { get; private set; }
        public int UpdateBy { get; private set; }

        public static UserSecurity Create(CreateUserSecurity command)
        {
            UserSecurity userSecurity = new UserSecurity();
            userSecurity.UserId = command.UserId;
            userSecurity.PasswordHash = command.PasswordHash;
            userSecurity.CreateAt = command.CreateAt;
            userSecurity.CreateBy = command.CreateBy;
            userSecurity.UpdateAt = command.CreateAt;
            userSecurity.UpdateBy = command.CreateBy;
            return userSecurity;
        }

        public bool PasswordMatchWith(string passwordHash)
        {
            return this.PasswordHash == passwordHash;
        }

        public void ChangePassword(UserSecurityCommandBase command)
        {
            this.PasswordHash = command.PasswordHash;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }
    }
}
