using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceContract]
    internal interface IServerTaskService
    {
        [OperationContract]
        int CreateTask(DistributedTaskLibrary libraries);
        [OperationContract]
        List<DistributedComputingResult> ExecuteTask(Uri localUri, int taskid, DividedResource resource);
        [OperationContract]
        DistributedResource Execute(Uri localUri, int TaskId, DistributedResource resource);
    }
}
