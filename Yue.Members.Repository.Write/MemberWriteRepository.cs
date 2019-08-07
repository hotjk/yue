using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Members.Contract;
using Yue.Members.Handler;
using Yue.Members.Model;
using Dapper;
using Yue.Common.Repository;

namespace Yue.Members.Repository.Write
{
    public class MemberWriteRepository : BaseRepository, IMemberWriteRepository
    {
        public MemberWriteRepository(SqlOption option) : base(option) { }

        private static readonly string[] _memberColumns = new string[] {
"MemberId", "UserId", "GroupId", "MemberGroup", "CreateBy", "CreateAt" };

        public bool Add(Member member)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `members` ({0}) VALUES ({1});",
                    SqlHelper.Columns(_memberColumns),
                    SqlHelper.Params(_memberColumns)),
                    member);
            }
        }

        public bool AddAction(MemberCommandBase action)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Member member)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"DELETE `members` WHERE MemberId = @MemberId;"),
                    member);
            }
        }

        public Member GetForUpdate(int memberId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var member = connection.Query<Member>(
                     string.Format(@"SELECT {0} FROM `members` WHERE `MemberId` = @MemberId FOR UPDATE;",
                     SqlHelper.Columns(_memberColumns)),
                     new { MemberId = memberId }).SingleOrDefault();
                return member;
            }
        }
    }
}
