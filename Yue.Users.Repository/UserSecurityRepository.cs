using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Repository;
using Yue.Users.Model;
using Dapper;

namespace Yue.Users.Repository
{
    public class UserSecurityRepository : BaseRepository, IUserSecurityRepository
    {
        public UserSecurityRepository(SqlOption option) : base(option) { }

        private static readonly string[] _userColumns = new string[] {
"UserId", "Email", "Name", "State", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private static readonly string[] _userSecurityColumns = new string[] {
"UserId", "PasswordHash", "ActivateToken", "ResetPasswordToken", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };

        public UserSecurity Get(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<UserSecurity, User, UserSecurity>(
                     string.Format(@"SELECT {0},{1} FROM `user_security` JOIN `users` ON `user_security`.`UserId` = `users`.`UserId` WHERE `user_security`.`UserId` = @UserId;",
                     SqlHelper.Columns("user_security", _userSecurityColumns),
                     SqlHelper.Columns("users", _userColumns)),
                     (UserSecurity, user) => { UserSecurity.User = user; return UserSecurity; },
                     new { UserId = userId }, splitOn: "UserId").SingleOrDefault();
            }
        }

        public UserSecurity UserSecurityByEmail(string email)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<UserSecurity, User, UserSecurity>(
                     string.Format(@"SELECT {0}, {1} FROM `user_security` JOIN `users` ON `user_security`.`UserId` = `users`.`UserId` WHERE `users`.`Email` = @Email;",
                     SqlHelper.Columns("user_security", _userSecurityColumns),
                     SqlHelper.Columns("users", _userColumns)),
                     (UserSecurity, user) => { UserSecurity.User = user; return UserSecurity; },
                     new { Email = email }, splitOn: "UserId").SingleOrDefault();
            }
        }
    }
}
