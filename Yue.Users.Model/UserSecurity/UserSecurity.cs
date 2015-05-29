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

        public User User { get; set; }
        
        private static StateMachine<UserState, UserSecurityCommand> _fsm;

        static UserSecurity()
        {
            _fsm = new StateMachine<UserState, UserSecurityCommand>();
            _fsm.Configure(UserState.Initial)
                .Permit(UserSecurityCommand.CreateUserSecurity, UserState.Initial);
            _fsm.Configure(UserState.Inactive)
                .Permit(UserSecurityCommand.CreateUserSecurity, UserState.Inactive)
                .Permit(UserSecurityCommand.VerifyPassword, UserState.Inactive)
                .Permit(UserSecurityCommand.RequestActivateToken, UserState.Inactive)
                .Permit(UserSecurityCommand.ResetActivateToken, UserState.Normal)
                .Permit(UserSecurityCommand.ChangePassword, UserState.Inactive)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed);
            _fsm.Configure(UserState.Normal)
                .Permit(UserSecurityCommand.VerifyPassword, UserState.Normal)
                .Permit(UserSecurityCommand.RequestResetPasswordToken, UserState.Normal)
                .Permit(UserSecurityCommand.VerifyResetPasswordToken, UserState.Normal)
                .Permit(UserSecurityCommand.CancelResetPasswordToken, UserState.Normal)
                .Permit(UserSecurityCommand.ResetPassword, UserState.Normal)
                .Permit(UserSecurityCommand.ChangePassword, UserState.Normal)
                .Permit(UserSecurityCommand.Block, UserState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed);
            _fsm.Configure(UserState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed)
                .Permit(UserSecurityCommand.Restore, UserState.Normal);
        }

        public bool EnsoureState(UserSecurityCommand action)
        {
            return _fsm.Instance(User.State).CanFire(action);
        }

        public void EnsoureAndUpdateState(object action)
        {
            var command = (UserSecurityCommand)Enum.Parse(typeof(UserSecurityCommand), action.GetType().Name, true);

            var instance = _fsm.Instance(User.State);
            if (!instance.Fire(command))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid user state.");
            }
        }

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

        public void UpdatePassword(ChangePassword command)
        {
            this.PasswordHash = command.PasswordHash;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public void UpdatePassword(ResetPassword command)
        {
            this.PasswordHash = command.PasswordHash;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public bool VerifyActivateToken(string token)
        {
            return (!string.IsNullOrEmpty(this.ActivateToken) &&
                string.Compare(this.ActivateToken, token) == 0);
        }

        public void UpdateActivateToken(RequestActivateToken command)
        {
            this.ActivateToken = command.Token;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public void ClearActivateToken(ResetActivateToken command)
        {
            this.ActivateToken = null;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public bool VerifyResetPasswordToken(string token)
        {
            return (!string.IsNullOrEmpty(this.ResetPasswordToken) &&
                string.Compare(this.ResetPasswordToken, token) == 0);
        }

        public void UpdateResetPasswordToken(RequestResetPasswordToken command)
        {
            this.ResetPasswordToken = command.Token;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public void ClearResetPasswordToken(ResetPassword command)
        {
            this.ResetPasswordToken = null;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }

        public void ClearResetPasswordToken(CancelResetPasswordToken command)
        {
            this.ResetPasswordToken = null;
            this.UpdateAt = command.CreateAt;
            this.UpdateBy = command.CreateBy;
        }
    }
}
