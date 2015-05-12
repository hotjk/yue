using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class Register : UserActionBase
    {
        public Register(int userId, string email, string name,
            string passwordHash, DateTime createAt)
        {
            this.UserId = userId;
            this.Email = email;
            this.Name = name;
            this.PasswordHash = passwordHash;
            this.CreateAt = createAt;
        }

        public int UserId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreateAt { get; set; }
    }
}
