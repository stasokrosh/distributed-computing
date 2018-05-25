using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceContract]
    internal interface IWorkerPerformanceService
    {
        [OperationContract]
        int GetPerformance();
    }
}
