using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DistributedComputing
{
    [DataContract]
    internal class DistributedComputingResult
    {
        [DataMember]
        public DistributedResource Data;
        [DataMember]
        public DistributedResource Result;
    }
}
