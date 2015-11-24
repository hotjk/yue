using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Repository;
using Yue.Members.Contract;
using Yue.Members.Model;
using Dapper;

namespace Yue.Members.Repository
{
    public class MemberRepository : BaseRepository, IMemberRepository
    {
        public MemberRepository(SqlOption option) : base(option) { }

        private static readonly string[] _memberColumns = new string[] {
"MemberId", "UserId", "GroupId", "MemberGroup", "CreateBy", "CreateAt" };

        public Member Get(int memberId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                Member member = connection.Query<Member>(
                    string.Format(
@"SELECT {0} FROM `members` 
WHERE `MemberId` = @MemberId;", SqlHelper.Columns(_memberColumns)),
                    new { MemberId = memberId }).SingleOrDefault();
                return member;
            }
        }

        public GroupMembers MembersByGroup(MemberGroup group, int groupId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var members = connection.Query<Member>(
                    string.Format(
@"SELECT {0} FROM `members` 
WHERE `GroupId` = @GroupId
AND `MemberGroup` = @MemberGroup;", SqlHelper.Columns(_memberColumns)),
                    new { GroupId = groupId, MemberGroup = group });
                return new GroupMembers(group, groupId, members.ToList());
            }
        }

        public Member UserInGroup(MemberGroup group, int groupId, int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var member = connection.Query<Member>(
                    string.Format(
@"SELECT {0} FROM `members` 
WHERE `GroupId` = @GroupId
AND `MemberGroup` = @MemberGroup
AND `UserId` = @UserId;", SqlHelper.Columns(_memberColumns)),
                    new { GroupId = groupId, MemberGroup = group, UserId = userId }).SingleOrDefault();
                return member;
            }
        }
    }
}
