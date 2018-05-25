using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization;

namespace DistributedComputing
{
    [DataContract]
    internal class DistributedTaskLibrary
    {
        [DataMember]
        public string ProcessTypeName { get; private set; }
        [DataMember]
        public Uri Holder { get; private set; }
        [DataMember]
        public List<string> Files { get; private set; }
        public DistributedTaskLibrary(Uri holder,List<string> files,string name)
        {
            Files = files;
            Holder = holder;
            ProcessTypeName = name;
        }
    }
}
