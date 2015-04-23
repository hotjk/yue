using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Common.Contract
{
    public class PagedResponseParams
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public int? Total { get; set; }
        public bool? More { get; set; }
    }
}
