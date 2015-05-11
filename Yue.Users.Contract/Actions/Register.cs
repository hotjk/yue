using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class Register : UserActionBase
    {
        public int UserId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string PasswordHash { get; private set; }

        public DateTime RegisterAt { get; set; }
    }
}
