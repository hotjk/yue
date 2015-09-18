using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Resources.Contract;

namespace Yue.Resources.Model
{
    public class GroupMembers
    {
        public int GroupId { get; private set; }
        public MemberGroup MemberGroup { get; private set; }
        public ICollection<Member> Members { get; private set; }
    }
}
