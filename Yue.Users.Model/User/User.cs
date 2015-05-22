using ACE.Exceptions;
using Grit.Pattern.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Users.Contract;
using Yue.Users.Model.Commands;

namespace Yue.Users.Model
{
    public class User : IAggregateRoot
    {
        public int UserId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public UserState State { get; private set; }
        public string ActivateToken { get; private set; }

        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int UpdateBy { get; private set; }
        public DateTime UpdateAt { get; private set; }

        private static StateMachine<UserState, UserCommand> _fsmUser;
        private static StateMachine<UserState, UserSecurityCommand> _fsmUserSecurity;

        static User()
        {
            InitStateMachine();
        }

        private static void InitStateMachine()
        {
            _fsmUser = new StateMachine<UserState, UserCommand>();
            _fsmUser.Configure(UserState.Initial)
                .Permit(UserCommand.Create, UserState.Inactive);
            _fsmUser.Configure(UserState.Inactive)
                .Permit(UserCommand.ChangeProfile, UserState.Inactive);
            _fsmUser.Configure(UserState.Normal)
                .Permit(UserCommand.ChangeProfile, UserState.Normal);

            _fsmUserSecurity = new StateMachine<UserState, UserSecurityCommand>();
            _fsmUserSecurity.Configure(UserState.Initial)
                .Permit(UserSecurityCommand.CreateUserSecurity, UserState.Initial);
            _fsmUserSecurity.Configure(UserState.Inactive)
                .Permit(UserSecurityCommand.CreateUserSecurity, UserState.Inactive)
                .Permit(UserSecurityCommand.VerifyPassword, UserState.Inactive)
                .Permit(UserSecurityCommand.RequestActivateToken, UserState.Inactive)
                .Permit(UserSecurityCommand.ActivateUser, UserState.Normal)
                .Permit(UserSecurityCommand.ChangePassword, UserState.Inactive)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed);
            _fsmUserSecurity.Configure(UserState.Normal)
                .Permit(UserSecurityCommand.VerifyPassword, UserState.Normal)
                .Permit(UserSecurityCommand.RequestResetPasswordToken, UserState.Normal)
                .Permit(UserSecurityCommand.VerifyResetPasswordToken, UserState.Normal)
                .Permit(UserSecurityCommand.CancelResetPasswordToken, UserState.Normal)
                .Permit(UserSecurityCommand.ResetPassword, UserState.Normal)
                .Permit(UserSecurityCommand.ChangePassword, UserState.Normal)
                .Permit(UserSecurityCommand.Block, UserState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed);
            _fsmUserSecurity.Configure(UserState.Blocked)
                .Permit(UserSecurityCommand.Destory, UserState.Destroyed)
                .Permit(UserSecurityCommand.Restore, UserState.Normal);
        }

        public bool EnsoureState(UserCommand action)
        {
            return _fsmUser.Instance(this.State).CanFire(action);
        }

        public void EnsoureAndUpdateState(UserCommandBase action)
        {
            var instance = _fsmUser.Instance(this.State);
            if (!instance.Fire(action.Type))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid user state.");
            }
            this.State = instance.State;
            this.UpdateBy = action.CreateBy;
            this.UpdateAt = action.CreateAt;
        }

        public bool EnsoureState(UserSecurityCommand action)
        {
            return _fsmUserSecurity.Instance(this.State).CanFire(action);
        }

        public void EnsoureAndUpdateState(UserSecurityCommandBase action)
        {
            var instance = _fsmUserSecurity.Instance(this.State);
            if (!instance.Fire(action.Type))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid user state.");
            }
            this.State = instance.State;
            this.UpdateBy = action.CreateBy;
            this.UpdateAt = action.CreateAt;
        }

        public static User Create(CreateUser command)
        {
            User user = new User();
            user.State = UserState.Initial;

            user.EnsoureAndUpdateState(command);
            user.UserId = command.UserId;
            user.Name = command.Name;
            user.Email = command.Email;
            user.CreateAt = command.CreateAt;
            user.CreateBy = command.CreateBy;
            user.UpdateAt = command.CreateAt;
            user.UpdateBy = command.CreateBy;

            return user;
        }
    }
}
