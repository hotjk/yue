using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Repository;
using Yue.Users.Model;
using Dapper;
using Yue.Users.Repository.Model;
using Yue.Users.Contract.Commands;
using Yue.Users.Handler;

namespace Yue.Users.Repository.Write
{
    public class UserSecurityWriteRepository : BaseRepository, IUserSecurityWriteRepository
    {
        public UserSecurityWriteRepository(SqlOption option) : base(option) { }

        private static readonly string[] _userColumns = new string[] {
"UserId", "Email", "Name", "State", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private static readonly string[] _userSecurityColumns = new string[] {
"UserId", "PasswordHash", "ActivateToken", "ResetPasswordToken", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private static readonly string[] _userSecurityUpdateColumns = new string[] {
"PasswordHash", "ActivateToken", "ResetPasswordToken", "UpdateAt", "UpdateBy" };
        private static readonly string[] _userSecurityLogsColumns = new string[] {
"UserId", "Type", "PasswordHash", "Token", "CreateBy", "CreateAt" };

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

        public UserSecurity GetForUpdate(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<UserSecurity, User, UserSecurity>(
                     string.Format(@"SELECT {0},{1} FROM `user_security` JOIN `users` ON `user_security`.`UserId` = `users`.`UserId` WHERE `user_security`.`UserId` = @UserId FOR UPDATE;",
                     SqlHelper.Columns("user_security", _userSecurityColumns),
                     SqlHelper.Columns("users", _userColumns)),
                     (UserSecurity, user) => { UserSecurity.User = user; return UserSecurity; },
                     new { UserId = userId }, splitOn: "UserId").SingleOrDefault();
            }
        }

        public bool Add(UserSecurity userSecurity)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `user_security` ({0}) VALUES ({1});",
                    SqlHelper.Columns(_userSecurityColumns),
                    SqlHelper.Params(_userSecurityColumns)),
                    userSecurity);
            }
        }

        public bool Update(UserSecurity userSecurity)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"UPDATE `user_security` SET {0} WHERE `UserId` = @UserId;",
                    SqlHelper.Sets(_userSecurityUpdateColumns)),
                    userSecurity);
            }
        }

        public bool Log(UserSecurityCommandBase command)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `user_security_logs` ({0}) VALUES ({1});",
                    SqlHelper.Columns(_userSecurityLogsColumns),
                    SqlHelper.Params(_userSecurityLogsColumns)),
                    UserSecurityLogPM.ToPM(command));
            }
        }
    }
}
