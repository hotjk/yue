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

        private static readonly string[] _userSecurityColumns = new string[] {
"UserId", "PasswordHash", "PasswordChangeAt", "PasswordChangeBy" };

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
    }
}
