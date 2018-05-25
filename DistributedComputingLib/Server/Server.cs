using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel;

namespace DistributedComputing
{
    public class Server : IDisposable
    {
        private bool ClientConnectionOpen = false;
        private int Port = 9000;
        public Uri ServerUri { get; private set; }
        private ServiceHost TaskServiceHost, ClientRegistrationHost, WorkerRegistrationHost, TaskLibraryHost, CheckConnectionHost;
        private ServerTaskService TaskService;
        private ClientRegistrationService ClientRegistrationService;
        private WorkerRegistrationService WorkerRegistrationService;
        internal ServerConnectionControl ConnectionControl;
        internal TaskLibraryManager TaskLibraryManager { get; private set; }
        internal List<WorkerInfo> Workers;
        public List<Uri> Clients;
        public Server(IPAddress serverIp)
        {
            Workers = new List<WorkerInfo>();
            Clients = new List<Uri>();
            TaskService = new ServerTaskService(this);
            ClientRegistrationService = new ClientRegistrationService(this);
            WorkerRegistrationService = new WorkerRegistrationService(this);
            ConnectionControl = new ServerConnectionControl(this);
            TaskLibraryManager = new TaskLibraryManager();
            TaskLibraryManager.Load();
            bool hostCreated = false;
            while (!hostCreated)
            {
                try
                {
                    ServerUri = RemoteConnection.CreateUri(serverIp, Port++);
                    CheckConnectionHost = RemoteConnection.CreateHost(UriTemplates.CreateCheckConnectionUri(ServerUri), typeof(ICheckConnection), typeof(CheckConnection));
                    hostCreated = true;
                }
                catch
                {
                }
            }
            TaskServiceHost = RemoteConnection.CreateHost(UriTemplates.CreateTasksUri(ServerUri), typeof(IServerTaskService), TaskService);
            ClientRegistrationHost = RemoteConnection.CreateHost(UriTemplates.CreateClientRegistrationUri(ServerUri), typeof(IClientRegistrationService), ClientRegistrationService);
            WorkerRegistrationHost = RemoteConnection.CreateHost(UriTemplates.CreateWorkerRegistrationUri(ServerUri), typeof(IWorkerRegistrationService), WorkerRegistrationService);
            TaskLibraryHost = RemoteConnection.CreateHost(UriTemplates.CreateLibrariesUri(ServerUri), typeof(IDistributedTaskLibraryService), typeof(DistributedTaskLibraryService));
            CheckConnectionHost = RemoteConnection.CreateHost(UriTemplates.CreateCheckConnectionUri(ServerUri), typeof(ICheckConnection), typeof(CheckConnection));
            CheckConnectionHost.Open();
            WorkerRegistrationHost.Open();
            TaskLibraryHost.Open();
        }
        public void OpenClientConnection()
        {
            ClientRegistrationHost.Open();
            TaskServiceHost.Open();
            ClientConnectionOpen = true;
        }
        public void CloseClientConnection()
        {
            ClientRegistrationHost.Close();
            TaskServiceHost.Close();
            ClientConnectionOpen = false;
        }
        public void Dispose()
        {
            TaskLibraryManager.Save();
            WorkerRegistrationHost.Close();
            TaskLibraryHost.Close();
            CheckConnectionHost.Close();
            if (ClientConnectionOpen)
            {
                CloseClientConnection();
            }
        }
    }
}
