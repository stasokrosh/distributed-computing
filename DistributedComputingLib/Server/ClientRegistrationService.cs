using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class ClientRegistrationService : IClientRegistrationService
    {
        private Server Server;
        public ClientRegistrationService(Server server)
        {
            Server = server;
        }
        public void Connect(Uri localuri)
        {
            lock (Server.Clients)
            {
                Server.Clients.Add(localuri);
            }
            lock (Server.Workers)
            {
                foreach (WorkerInfo workerinfo in Server.Workers)
                {
                    if (!workerinfo.Connections.Select<ConnectionInfo, Uri>(connection => connection.ClientUri).Contains(localuri))
                    {
                        var service = RemoteConnection.CreateProxy<IWorkerTaskService>(UriTemplates.CreateTasksUri(workerinfo.WorkerUri));
                        workerinfo.Connections.Add(new ConnectionInfo(localuri, service.CheckConnection(localuri)));
                    }
                }
            }
            Server.ConnectionControl.AddClientConnection(localuri);
            Logging.WriteLog("Client on " + localuri + " connected");
        }
        public void Disconnect(Uri localuri)
        {
            lock (Server.Clients)
            {
                Server.Clients.Remove(localuri);
            }
            Server.ConnectionControl.DeleteClientConnection(localuri);
            Logging.WriteLog("Client on " + localuri + " disconnected");
        }
    }
}
