﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Users.Contract.Actions
{
    public class Activate : UserActionBase
    {
        public Activate(int userId, string token, DateTime createAt, int createBy)
        {
            this.UserId = userId;
            this.Token = token;
            this.CreateAt = createAt;
            this.CreateBy = createBy;
        }
        public int UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }
    }
}
