using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class Register : PasswordUserActionBase
    {
        public Register(int userId, DateTime createAt, int createBy, string passwordHash,
            string email, string name)
            : base(userId, createAt, createBy, passwordHash)
        {
            this.Email = email;
            this.Name = name;
        }

        public string Email { get; private set; }
        public string Name { get; private set; }
    }
}
