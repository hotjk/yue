using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Resources.Model
{
    public interface IMemberRepository
    {
        Member MemberById(int memberId);
        GroupMembers MembersByResource(int resourceId);
        GroupMembers MembersByOrganization(int resourceId);
    }
}
