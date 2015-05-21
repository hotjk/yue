﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class ChangePassword : PasswordUserActionBase
    {
        public ChangePassword(int userId, DateTime createAt, int createBy, string passwordHash)
            :base(userId, createAt, createBy, passwordHash)
        {
        }
    }
}
