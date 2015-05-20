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
        public string PasswordHash { get; private set; }
        public string ActivateToken { get; private set; }
        public string ResetPasswordToken { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
        public DateTime UpdateAt { get; set; }
        public int UpdateBy { get; set; }

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

        public void UpdatePassword(PasswordCommandBase command)
        {
            this.PasswordHash = command.PasswordHash;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public void UpdateActivateToken(TokenCommandBase command)
        {
            this.ActivateToken = command.Token;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public void UpdateResetPasswordToken(TokenCommandBase command)
        {
            this.ResetPasswordToken = command.Token;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }
    }
}
