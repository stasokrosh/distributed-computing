using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceContract]
    internal interface IWorkerTaskService
    {
        [OperationContract]
        int CreateTask(DistributedTaskLibrary libraries);
        [OperationContract]
        DistributedResource ExecuteTask(int taskId, DistributedResource resource);
        [OperationContract]
        int CheckConnection(Uri clienturi);
    }
}
