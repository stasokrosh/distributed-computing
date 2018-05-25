using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    public interface IResultCompiler<T>
    {
        T CompileResult(List<T> resultFragments);
    }
}
