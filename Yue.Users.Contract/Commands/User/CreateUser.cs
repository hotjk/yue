using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Commands
{
    public class CreateUser : UserCommandBase, ACE.ICommand
    {
        public CreateUser(int userId, string email, string name, DateTime createAt, int createBy)
            :base(UserCommand.Create, createAt, createBy)
        {
            this.UserId = userId;
            this.Email = email;
            this.Name = name;
        }

        public int UserId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
        public string PasswordHash { get; private set; }
    }
}
