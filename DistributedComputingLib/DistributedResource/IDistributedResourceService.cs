using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceContract]
    internal interface IDistributedResourceService
    {
        [OperationContract]
        byte[] Get(int id);
        [OperationContract]
        void Delete(int id);
    }
}
