using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Runtime.Serialization;

namespace DistributedComputing
{
    [DataContract]
    public class DistributedResource
    {
        [DataMember]
        private bool Disposed = false;
        [DataMember]
        private Uri Holder;
        [DataMember]
        public int Id { get; private set;}
        internal DistributedResource(Uri holder, int id)
        {
            Holder = holder;
            Id = id;
        }
        public byte[] Get()
        {
            if (!Disposed)
            {
                var service = RemoteConnection.CreateProxy<IDistributedResourceService>(Holder);
                return service.Get(Id);
            }
            else
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }
        public void Dispose()
        {
            var service = RemoteConnection.CreateProxy<IDistributedResourceService>(Holder);
            service.Delete(Id);
            Disposed = true;
        }
    }
}
