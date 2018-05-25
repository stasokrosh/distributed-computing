using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel;

namespace DistributedComputing
{
    public class Worker : IDisposable
    {
        public Uri LocalUri { get; private set; }
        public Uri ServerUri { get; private set; }
        private WorkerServerConnection Connection;
        public Worker(IPAddress localIp,IPAddress serverIp,int port)
        {
            Connection = new WorkerServerConnection(localIp, serverIp, port);
            LocalUri = Connection.LocalUri;
            ServerUri = Connection.ServerUri;
        }
        public void Dispose()
        {
            Connection.Close();
        }
    }
}
