using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Members.Contract;
using Yue.Members.Model;

namespace Yue.Members.Handler
{
    public interface IMemberWriteRepository
    {
        Member GetForUpdate(int memberId);
        bool Add(Member member);
        bool Remove(Member member);
        bool AddAction(MemberCommandBase action);
    }
}
