﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class ResetPassword : PasswordUserActionBase
    {
        public ResetPassword(int userId, DateTime createAt, int createBy, string passwordHash, string token)
            :base(userId, createAt, createBy, passwordHash)
        {
            this.Token = token;
        }
        public string Token { get; private set; }
    }
}
