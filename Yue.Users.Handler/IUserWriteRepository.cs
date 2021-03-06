﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Model;

namespace Yue.Users.Handler
{
    public interface IUserWriteRepository
    {
        User Get(int userId);
        User UserByEmail(string email);
        User GetForUpdate(int userId);
        bool Add(User user);
        bool Update(User user);
    }
}
