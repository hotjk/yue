using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Resources.Contract
{
    public enum ResourcesState
    {
        Initial = 0,
        Normal = 1,
        Closed = 10,
        Destroyed = 20,
    }
}
