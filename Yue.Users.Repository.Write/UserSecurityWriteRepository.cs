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

namespace Yue.Users.Repository.Write
{
    public class UserSecurityWriteRepository : BaseRepository, IUserSecurityWriteRepository
    {
        public UserSecurityWriteRepository(SqlOption option) : base(option) { }

        private static readonly string[] _userSecurityColumns = new string[] {
"UserId", "PasswordHash", "PasswordChangeAt", "PasswordChangeBy" };
        private static readonly string[] _userSecurityUpdateColumns = new string[] {
"PasswordHash", "PasswordChangeAt", "PasswordChangeBy" };

        public UserSecurity UserSecurityByEmail(string email)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<UserSecurity>(
                     string.Format(@"SELECT {0} FROM `user_security` WHERE `Email` = @Email;",
                     SqlHelper.Columns(_userSecurityColumns)),
                     new { Email = email }).SingleOrDefault();
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

        public bool Update(User user)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"UPDATE `user_security` SET {0} WHERE `UserId` = @UserId;",
                    SqlHelper.Sets(_userSecurityUpdateColumns)),
                    user);
            }
        }

        public bool AddLog(UserSecurity userSecurity)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `user_security_logs` ({0}) VALUES ({1});",
                    SqlHelper.Columns(_userSecurityColumns),
                    SqlHelper.Params(_userSecurityColumns)),
                    userSecurity);
            }
        }
    }
}
