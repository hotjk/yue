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

        private static StateMachine<UserState, UserCommand> _fsm;

        static User()
        {
            _fsm = new StateMachine<UserState, UserCommand>();
            _fsm.Configure(UserState.Initial)
                .Permit(UserCommand.CreateUser, UserState.Inactive);
            _fsm.Configure(UserState.Inactive)
                .Permit(UserCommand.ActivateUser, UserState.Normal)
                .Permit(UserCommand.ChangeProfile, UserState.Inactive);
            _fsm.Configure(UserState.Normal)
                .Permit(UserCommand.ChangeProfile, UserState.Normal);
        }

        public bool EnsoureState(UserCommand action)
        {
            return _fsm.Instance(this.State).CanFire(action);
        }

        public void EnsoureAndUpdateState(object action)
        {
            var command = (UserCommand)Enum.Parse(typeof(UserCommand), action.GetType().Name, true);

            var instance = _fsm.Instance(this.State);
            if (!instance.Fire(command))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid user state.");
            }
            this.State = instance.State;
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
