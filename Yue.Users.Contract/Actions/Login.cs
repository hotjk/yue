using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions.User
{
    public class Login : UserActionBase
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime LoginAt { get; private set; }
        public string Ip { get; private set; }
    }
}
