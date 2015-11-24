namespace Yue.Members.Model
{
    public interface IMemberService
    {
        Member Get(int memberId);
        GroupMembers MembersByResource(int id);
        GroupMembers MembersByOrganization(int id);
        Member UserInResource(int resourceId, int userId);
        Member UserInOrganization(int orgId, int userId);
    }
}