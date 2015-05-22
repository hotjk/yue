using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract;

namespace Yue.Users.Model.Commands
{
    public class RequestActivateToken : TokenCommandBase
    {
        public RequestActivateToken(int userId, string token, DateTime createAt, int createBy)
            : base(userId, UserSecurityCommand.RequestActivateToken, token, createAt, createBy)
        {
        }
    }
}
