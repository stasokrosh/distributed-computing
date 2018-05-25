using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    public interface IProcess<T,V>
    {
        V Execute(T resource);
    }
}
