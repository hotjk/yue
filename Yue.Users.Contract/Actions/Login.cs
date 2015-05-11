﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class Login : UserActionBase
    {
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreateAt { get; private set; }
    }
}
