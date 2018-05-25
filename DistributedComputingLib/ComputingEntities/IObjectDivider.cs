using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    public interface IObjectDivider<T>
    {
        List<T> Divide(T resource);
    }
}
