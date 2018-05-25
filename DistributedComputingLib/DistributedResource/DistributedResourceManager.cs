using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    internal class DistributedResourceManager
    {
        private bool Disposed = false;
        private ServiceHost ServiceHost;
        private DistributedResourceHandler Handler;
        private Uri LocalUri;
        internal DistributedResourceManager(Uri localUri)
        {
            Handler = new DistributedResourceHandler();
            LocalUri = UriTemplates.CreateResourcesUri(localUri);
            ServiceHost = RemoteConnection.CreateHost(LocalUri, typeof(IDistributedResourceService), Handler);
            ServiceHost.Open();
        }
        public DistributedResource CreateDistributedResource(object resource)
        {
            int id = Handler.AddResource(resource);
            return new DistributedResource(LocalUri, id);
        }
        public void Dispose()
        {
            ServiceHost.Close();
            Disposed = true;
        }
        ~DistributedResourceManager()
        {
            if (!Disposed)
            {
                Dispose();
            }
        }
    }
}
