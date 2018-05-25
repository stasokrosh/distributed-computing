using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DistributedComputing
{
    [DataContract]
    internal class DividedResource
    {
        [DataMember]
        public List<DistributedResource> Resources;
        public void Dispose()
        {
            foreach (DistributedResource resource in Resources)
            {
                resource.Dispose();
            }
        }
    }
}
