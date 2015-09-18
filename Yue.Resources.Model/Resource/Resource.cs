using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Resources.Model
{
    public class Resource
    {
        public int ResourceId { get; private set; }
        public string Name { get; private set; }
        public int? OrganizationId { get; private set; }
    }
}
