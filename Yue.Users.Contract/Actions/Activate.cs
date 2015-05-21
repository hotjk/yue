using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class Activate : TokenUserActionBase
    {
        public Activate(int userId, DateTime createAt, int createBy, string token)
            :base(userId, createAt, createBy, token)
        {
        }
    }
}
