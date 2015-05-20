using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Repository;
using Yue.Users.Model;
using Yue.Users.Model.Write;
using Dapper;
using Yue.Users.Contract.Commands;
using Yue.Users.Repository.Model;

namespace Yue.Users.Repository.Write
{
    public class UserSecurityWriteRepository : BaseRepository, IUserSecurityWriteRepository
    {
        public UserSecurityWriteRepository(SqlOption option) : base(option) { }

        private static readonly string[] _userSecurityColumns = new string[] {
"UserId", "PasswordHash", "ActivateToken", "ResetPasswordToken", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private static readonly string[] _userSecurityUpdateColumns = new string[] {
"PasswordHash", "ActivateToken", "ResetPasswordToken", "UpdateAt", "UpdateBy" };
        private static readonly string[] _userSecurityLogsColumns = new string[] {
"UserId", "Type", "Data", "CreateBy", "CreateAt" };

        public UserSecurity Get(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<UserSecurity>(
                     string.Format(@"SELECT {0} FROM `user_security` WHERE `UserId` = @UserId;",
                     SqlHelper.Columns(_userSecurityColumns)),
                     new { UserId = userId }).SingleOrDefault();
            }
        }

        public UserSecurity GetForUpdate(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<UserSecurity>(
                     string.Format(@"SELECT {0} FROM `user_security` WHERE `UserId` = @UserId FOR UPDATE;",
                     SqlHelper.Columns(_userSecurityColumns)),
                     new { UserId = userId }).SingleOrDefault();
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
