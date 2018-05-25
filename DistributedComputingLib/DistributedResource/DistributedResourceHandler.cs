using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.IO;

namespace DistributedComputing
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class DistributedResourceHandler : IDistributedResourceService
    {
        private Dictionary<int, object> Resources;
        private int ResourceCount = -1;
        public DistributedResourceHandler()
        {
            Resources = new Dictionary<int, object>();
        }
        public int AddResource(object resource)
        {
            try
            {
                do
                {
                    ResourceCount++;
                }
                while (Resources.Keys.Contains(ResourceCount));
                Resources.Add(ResourceCount, resource);
            }
            catch
            {
                ResourceCount = 0;
                while (Resources.Keys.Contains(ResourceCount))
                {
                    ResourceCount++;
                }
                Resources.Add(ResourceCount, resource);
            }
            return ResourceCount;
        }
        public byte[] Get(int id)
        {
            MemoryStream stream = null; 
            var serializer = new DataContractSerializer(Resources[id].GetType());
            stream = new MemoryStream();
            serializer.WriteObject(stream, Resources[id]);
            return stream.ToArray();
        }
        public void Delete(int id)
        {
            Resources.Remove(id);
        }
    }
}
