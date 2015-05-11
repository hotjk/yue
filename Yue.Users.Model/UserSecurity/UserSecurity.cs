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
        public UserSecurityState State { get; private set; }
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

        private static StateMachine<UserSecurityState, UserSecurityCommand> _stateMachine;

        static UserSecurity()
        {
            InitStateMachine();
        }

        private static void InitStateMachine()
        {
            _stateMachine = new StateMachine<UserSecurityState, UserSecurityCommand>();

            _stateMachine.Configure(UserSecurityState.Initial)
                .Permit(UserSecurityCommand.Create, UserSecurityState.Normal);

            _stateMachine.Configure(UserSecurityState.Normal)
                .Permit(UserSecurityCommand.ChangePassword, UserSecurityState.Normal)
                .Permit(UserSecurityCommand.ResetPassword, UserSecurityState.Normal)
                .Permit(UserSecurityCommand.VerifyUserPassword, UserSecurityState.Normal)
                .Permit(UserSecurityCommand.Block, UserSecurityState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserSecurityState.Destroyed);

            _stateMachine.Configure(UserSecurityState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserSecurityState.Destroyed)
                .Permit(UserSecurityCommand.Restore, UserSecurityState.Normal);

            _stateMachine.Configure(UserSecurityState.Destroyed)
                .Permit(UserSecurityCommand.Restore, UserSecurityState.Normal);
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
