using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Members.Contract;

namespace Yue.Members.Model
{
    public class MemberService : IMemberService
    {
        private IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            this._memberRepository = memberRepository;
        }

        public Member Get(int memberId)
        {
            return _memberRepository.Get(memberId);
        }

        public GroupMembers MembersByResource(int id)
        {
            return _memberRepository.MembersByGroup(MemberGroup.Resource, id);
        }

        public GroupMembers MembersByOrganization(int id)
        {
            return _memberRepository.MembersByGroup(MemberGroup.Organization, id);
        }

        public Member UserInResource(int resourceId, int userId)
        {
            return _memberRepository.UserInGroup(MemberGroup.Resource, resourceId, userId);
        }

        public Member UserInOrganization(int orgId, int userId)
        {
            return _memberRepository.UserInGroup(MemberGroup.Organization, orgId, userId);
        }
    }
}
