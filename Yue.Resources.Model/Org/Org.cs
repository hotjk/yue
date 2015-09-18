using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Resources.Model
{
    public class Org
    {
        public int OrgId { get; private set; }
        public string Name { get; private set; }
        public ICollection<int> Resources { get; private set; }
    }
}
