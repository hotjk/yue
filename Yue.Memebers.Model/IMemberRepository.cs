using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Members.Contract;

namespace Yue.Members.Model
{
    public interface IMemberRepository
    {
        Member Get(int memberId);
        GroupMembers MembersByGroup(MemberGroup group, int groupId);
        Member UserInGroup(MemberGroup group, int groupId, int userId);
    }
}
