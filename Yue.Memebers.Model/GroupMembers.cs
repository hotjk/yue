using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Members.Contract;

namespace Yue.Members.Model
{
    public class GroupMembers
    {
        public GroupMembers(MemberGroup group, int groupId, ICollection<Member> members)
        {
            MemberGroup = group;
            GroupId = groupId;
            Members = members;
        }
        public MemberGroup MemberGroup { get; private set; }
        public int GroupId { get; private set; }
        public ICollection<Member> Members { get; private set; }
    }
}
