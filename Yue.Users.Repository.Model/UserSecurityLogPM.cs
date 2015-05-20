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
            Mapper.CreateMap<ChangePassword, UserSecurityLogPM>().ForMember(x => x.Data, opt => opt.MapFrom(n => n.PasswordHash));
            Mapper.CreateMap<CreateUserSecurity, UserSecurityLogPM>().ForMember(x => x.Data, opt => opt.MapFrom(n => n.PasswordHash));
            Mapper.CreateMap<ResetPassword, UserSecurityLogPM>().ForMember(x => x.Data, opt => opt.MapFrom(n => n.PasswordHash));
            Mapper.CreateMap<TokenCommandBase, UserSecurityLogPM>().ForMember(x => x.Data, opt => opt.MapFrom(n => n.Token));
        }

        public int UserId { get; private set; }
        public UserSecurityCommand Type { get; private set; }
        public string Data { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int CreateBy { get; private set; }

        public static UserSecurityLogPM ToPM(UserSecurityCommandBase m)
        {
            var pm = Mapper.Map<UserSecurityLogPM>(m);
            return pm;
        }
    }
}
