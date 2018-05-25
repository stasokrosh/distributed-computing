using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceContract]
    internal interface IWorkerRegistrationService
    {
        [OperationContract]
        void Connect(Uri clientUri);
        [OperationContract]
        void Disconnect(Uri clientUri);
    }
}
