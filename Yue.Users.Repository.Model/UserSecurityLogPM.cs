using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract;
using Yue.Users.Contract.Commands;

namespace Yue.Users.Repository.Model
{
    public class UserSecurityLogPM
    {
        static UserSecurityLogPM()
        {
            var ns = typeof(UserSecurityCommandBase).Namespace;
            foreach (string name in Enum.GetNames(typeof(UserSecurityCommand)))
            {
                Type source = typeof(UserSecurityCommandBase).Assembly.GetType(ns + "." + name);
                if(source == null) continue;
                // Assumed that UserSecurityCommandBase and concrete class in the same directory. 
                Mapper.CreateMap(source, typeof(UserSecurityLogPM))
                    .ForMember("Type", opt => opt.Ignore());
            }
        }

        public int UserId { get; private set; }
        public UserSecurityCommand Type { get; private set; }
        public string PasswordHash { get; private set; }
        public string Token { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }

        public static UserSecurityLogPM ToPM(UserSecurityCommandBase m)
        {
            var pm = Mapper.Map<UserSecurityLogPM>(m);
            pm.Type = (UserSecurityCommand)Enum.Parse(typeof(UserSecurityCommand), m.GetType().Name, true);
            return pm;
        }
    }
}
