using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Resources.Model;

namespace Yue.Resources.Handler
{
    public interface IMemberWriteRepository
    {
        Member Get(int memberId);
        bool Add(Member member);
        bool Remove(Member member);
    }
}
