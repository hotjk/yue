using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Resources.Model
{
    public class MemberService
    {
        private IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            this._memberRepository = memberRepository;
        }

        public Member MemberById(int memberId)
        {
            return _memberRepository.MemberById(memberId);
        }

        public GroupMembers MembersByResource(int resourceId)
        {
            return _memberRepository.MembersByResource(resourceId);
        }

        public GroupMembers MembersByOrganization(int resourceId)
        {
            return _memberRepository.MembersByOrganization(resourceId);
        }
    }
}
