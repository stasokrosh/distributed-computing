using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel;

namespace DistributedComputing
{
    internal class Connection : IDisposable
    {
        protected bool Disposed = false;
        private static int Port = 9000;
        private ServiceHost CheckConnectionHost;
        public Uri LocalUri { get; private set; }
        public Uri ServerUri { get; private set; }
        public Connection(IPAddress localIp, IPAddress serverIp, int serverPort)
        {
            bool hostCreated = false;
            while (!hostCreated)
            {
                try
                {
                    LocalUri = RemoteConnection.CreateUri(localIp, Port++);
                    CheckConnectionHost = RemoteConnection.CreateHost(UriTemplates.CreateCheckConnectionUri(LocalUri), typeof(ICheckConnection), typeof(CheckConnection));
                    CheckConnectionHost.Open();
                    hostCreated = true;
                }
                catch
                {
                }
            }
            ServerUri = RemoteConnection.CreateUri(serverIp, serverPort);
        }
        public void Dispose()
        {
            CheckConnectionHost.Close();
            Disposed = true;
        }
    }
}
