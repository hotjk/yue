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

        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int UpdateBy { get; private set; }
        public DateTime UpdateAt { get; private set; }

        private static StateMachine<UserState, UserCommand> _stateMachine;

        static User()
        {
            InitStateMachine();
        }

        private static void InitStateMachine()
        {
            _stateMachine = new StateMachine<UserState, UserCommand>();

            _stateMachine.Configure(UserState.Initial)
                .Permit(UserCommand.Create, UserState.Registered);

            _stateMachine.Configure(UserState.Registered)
                .Permit(UserCommand.Activate, UserState.Activated)
                .Permit(UserCommand.ChangeProfile, UserState.Initial);

            _stateMachine.Configure(UserState.Activated)
                .Permit(UserCommand.ChangeProfile, UserState.Activated);
        }

        public bool EnsoureState(UserCommand action)
        {
            return _stateMachine.Instance(this.State).CanFire(action);
        }

        public void EnsoureAndUpdateState(UserCommand action)
        {
            var instance = _stateMachine.Instance(this.State);
            if (!instance.Fire(action))
            {
                throw new BusinessException(BusinessStatusCode.Forbidden, "Invalid user state.");
            }
            this.State = instance.State;
        }

        public static User Create(CreateUser command)
        {
            User user = new User();
            user.State = UserState.Initial;

            user.EnsoureAndUpdateState(command.Type);
            user.UserId = command.UserId;
            user.Name = command.Name;
            user.Email = command.Email;

            return user;
        }
    }
}
