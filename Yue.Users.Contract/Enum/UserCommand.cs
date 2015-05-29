using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract
{
    public enum UserCommand
    {
        CreateUser = 0,
        ActivateUser = 1,
        ChangeProfile = 10,
    }
}
