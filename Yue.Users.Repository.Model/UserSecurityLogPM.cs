using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Users.Contract;
using Yue.Users.Model.Commands;

namespace Yue.Users.Repository.Model
{
    public class UserSecurityLogPM
    {
        static UserSecurityLogPM()
        {
            Mapper.CreateMap<Activate, UserSecurityLogPM>();
            Mapper.CreateMap<CancelResetPasswordToken, UserSecurityLogPM>();
            Mapper.CreateMap<ChangePassword, UserSecurityLogPM>();
            Mapper.CreateMap<CreateUserSecurity, UserSecurityLogPM>();
            Mapper.CreateMap<RequestActivateToken, UserSecurityLogPM>();
            Mapper.CreateMap<RequestResetPasswordToken, UserSecurityLogPM>();
            Mapper.CreateMap<ResetPassword, UserSecurityLogPM>();
            Mapper.CreateMap<VerifyResetPasswordToken, UserSecurityLogPM>();
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
            return pm;
        }
    }
}
