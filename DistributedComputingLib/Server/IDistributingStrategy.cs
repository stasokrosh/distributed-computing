using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    internal interface IDistributingStrategy
    {
        List<TaskExecutor> Distribute(Uri ClientUri, List<DistributedResource> resources, List<WorkerInfo> workers);
    }
}
