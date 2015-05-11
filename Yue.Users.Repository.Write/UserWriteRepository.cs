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
    public class UserWriteRepository : BaseRepository, IUserWriteRepository
    {
        public UserWriteRepository(SqlOption option) : base(option) { }

        private static readonly string[] _userColumns = new string[] {
"UserId", "Email", "Name", "State", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private static readonly string[] _userUpdateColumns = new string[] {
"Email", "Name", "State", "UpdateBy", "UpdateAt" };

        private static readonly string[] _userSecurityColumns = new string[] {
"UserId", "PasswordHash", "PasswordChangeAt", "PasswordChangeBy" };

        public User GetForUpdate(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var user = connection.Query<User>(
                     string.Format(@"SELECT {0} FROM `users` WHERE `UserId` = @UserId FOR UPDATE;",
                     SqlHelper.Columns(_userColumns)),
                     new { UserId = userId }).SingleOrDefault();
                return user;
            }
        }

        public bool Add(User user)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `users` ({0}) VALUES ({1});",
                    SqlHelper.Columns(_userColumns),
                    SqlHelper.Params(_userColumns)),
                    user);
            }
        }

        public bool Update(User user)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"UPDATE `users` SET {0} WHERE `UserId` = @UserId;",
                    SqlHelper.Sets(_userUpdateColumns)),
                    user);
            }
        }
    }
}
