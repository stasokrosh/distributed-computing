using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class WorkerRegistrationService : IWorkerRegistrationService
    {
        private Server Server;
        public WorkerRegistrationService(Server server)
        {
            Server = server;
        }
        public void Connect(Uri workerUri)
        {
            var workerInfo = new WorkerInfo(workerUri);
            var service = RemoteConnection.CreateProxy<IWorkerTaskService>(UriTemplates.CreateTasksUri(workerUri));
            foreach (Uri client in Server.Clients)
            {
                workerInfo.Connections.Add(new ConnectionInfo(client, service.CheckConnection(client)));
            }
            for (int i=0;i<Server.TaskLibraryManager.Tasks.Count;i++)
            {
                workerInfo.Tasks.Add(i,service.CreateTask(Server.TaskLibraryManager.Tasks[i].CreateDistributedTaskLibrary(Server.ServerUri)));
            }
            lock (Server.Workers)
            {
                if (Server.Workers.Count == 0)
                {
                    Server.OpenClientConnection();
                }
                Server.Workers.Add(workerInfo);
            }
            Server.ConnectionControl.AddWorkerConnection(workerUri);
            Logging.WriteLog("Worker on " + workerUri + " connected");
        }
        public void Disconnect(Uri workerUri)
        {
            lock (Server.Workers)
            {
                Server.Workers.Remove(Server.Workers.Find(info => info.WorkerUri == workerUri));
                if (Server.Workers.Count == 0)
                {
                    Server.CloseClientConnection();
                }
            }
            Server.ConnectionControl.DeleteWorkerConnection(workerUri);
            Logging.WriteLog("Worker on " + workerUri + " connected");
        }
    }
}
