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
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(SqlOption option) : base(option) { }

        private static readonly string[] _userColumns = new string[] {
"UserId", "Email", "Name", "State", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };

        public User Get(int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var user = connection.Query<User>(
                     string.Format(@"SELECT {0} FROM `users` WHERE `UserId` = @UserId;",
                     SqlHelper.Columns(_userColumns)),
                     new { UserId = userId }).SingleOrDefault();
                return user;
            }
        }

        public User UserByEmail(string email)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var user = connection.Query<User>(
                     string.Format(@"SELECT {0} FROM `users` WHERE `UserId` = @UserId;",
                     SqlHelper.Columns(_userColumns)),
                     new { Email = email }).SingleOrDefault();
                return user;
            }
        }

        public IEnumerable<User> Users(IEnumerable<int> ids)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var users = connection.Query<User>(
                     string.Format(@"SELECT {0} FROM `users` WHERE `UserId` in @ids;",
                     SqlHelper.Columns(_userColumns)),
                     new { ids = ids });
                return users;
            }
        }
    }
}
