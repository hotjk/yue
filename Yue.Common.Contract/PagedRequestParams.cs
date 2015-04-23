using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Common.Contract
{
    public class PagedRequestParams
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
