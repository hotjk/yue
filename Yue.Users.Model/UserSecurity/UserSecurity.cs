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
        public DateTime PasswordChangeAt { get; private set; }
        public int PasswordChangeBy { get; private set; }

        public static UserSecurity Create(CreateUserSecurity command)
        {
            UserSecurity userSecurity = new UserSecurity();
            userSecurity.UserId = command.UserId;
            userSecurity.PasswordHash = command.PasswordHash;
            userSecurity.PasswordChangeAt = command.CreateAt;
            userSecurity.PasswordChangeBy = command.CreateBy;
            return userSecurity;
        }

        private static StateMachine<UserState, UserSecurityCommand> _stateMachine;

        static UserSecurity()
        {
            InitStateMachine();
        }

        private static void InitStateMachine()
        {
            _stateMachine = new StateMachine<UserState, UserSecurityCommand>();

            _stateMachine.Configure(UserState.Initial)
                .Permit(UserSecurityCommand.Create, UserState.Inactive);

            _stateMachine.Configure(UserState.Inactive)
                .Permit(UserSecurityCommand.Activate, UserState.Normal)
                .Permit(UserSecurityCommand.ChangePassword, UserState.Inactive)
                .Permit(UserSecurityCommand.ResetPassword, UserState.Inactive)
                .Permit(UserSecurityCommand.VerifyUserPassword, UserState.Inactive)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed);

            _stateMachine.Configure(UserState.Normal)
                .Permit(UserSecurityCommand.ChangePassword, UserState.Normal)
                .Permit(UserSecurityCommand.ResetPassword, UserState.Normal)
                .Permit(UserSecurityCommand.VerifyUserPassword, UserState.Normal)
                .Permit(UserSecurityCommand.Block, UserState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed);

            _stateMachine.Configure(UserState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed)
                .Permit(UserSecurityCommand.Restore, UserState.Normal);
        }

        public bool EnsoureState(UserSecurityCommand action)
        {
            return _stateMachine.Instance(this.State).CanFire(action);
        }

        public void EnsoureAndUpdateState(UserSecurityCommand action)
        {
            var instance = _stateMachine.Instance(this.State);
            if (!instance.Fire(action))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid user state.");
            }
            this.State = instance.State;
        }

        public bool PasswordMatchWith(string passwordHash)
        {
            return this.PasswordHash == passwordHash;
        }
    }
}
