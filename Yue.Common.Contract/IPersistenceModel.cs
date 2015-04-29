using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yue.Common.Contract
{
    public interface IPersistenceModel
    {
        void Persist();
        void Rehydrate();
    }
}
