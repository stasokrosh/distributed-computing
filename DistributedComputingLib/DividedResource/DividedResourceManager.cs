using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    internal class DividedResourceManager<T,Divider> : IDisposable where Divider : IObjectDivider<T>,new()
    {
        private bool Disposed = false;
        private DistributedResourceManager ResourceManager;
        private Divider ObjectDivider;
        public DividedResourceManager(Uri localUri)
        {
            ObjectDivider = new Divider();
            ResourceManager = new DistributedResourceManager(localUri);
        }
        public DividedResource CreateDividedResource(T resource)
        {
            if (!Disposed)
            {
                List<T> list = ObjectDivider.Divide(resource);
                List<DistributedResource> result = new List<DistributedResource>();
                foreach (T item in list)
                {
                    result.Add(ResourceManager.CreateDistributedResource(item));
                }
                return new DividedResource() { Resources = result };
            }
            else
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }
        public void Dispose()
        {
            ResourceManager.Dispose();
            Disposed = true;
        }
    }
}
