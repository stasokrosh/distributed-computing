using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel;

namespace DistributedComputing
{
    internal class WorkerServerConnection : Connection
    {
        private ServiceHost TasksServiceHost;
        private ServiceHost PerformanceServiceHost;
        private WorkerTaskService TasksService;
        private WorkerPerformanceService PerformanceService;
        private ConnectionControl ConnectionControl;
        public WorkerServerConnection(IPAddress localIp,IPAddress serverIp,int port) : base(localIp,serverIp,port)
        {
            Logging.WriteLog("Connecting to server on " + ServerUri);
            PerformanceService = new WorkerPerformanceService();
            PerformanceServiceHost = RemoteConnection.CreateHost(UriTemplates.CreatePerformanceUri(LocalUri),typeof(IWorkerPerformanceService),PerformanceService);
            PerformanceServiceHost.Open();
            TasksService = new WorkerTaskService(LocalUri);
            TasksServiceHost = RemoteConnection.CreateHost(UriTemplates.CreateTasksUri(LocalUri),typeof(IWorkerTaskService),TasksService);
            TasksServiceHost.Open();
            var registation = RemoteConnection.CreateProxy<IWorkerRegistrationService>(UriTemplates.CreateWorkerRegistrationUri(ServerUri));
            try
            {
                registation.Connect(LocalUri);
            }
            catch (Exception e)
            {
                TasksServiceHost.Close();
                PerformanceServiceHost.Close();
                Dispose();
                throw new NoServerConnectionException();
            }
            ConnectionControl = new ConnectionControl(this, ServerUri);
        }
        public void Close()
        {
            var registration = RemoteConnection.CreateProxy<IWorkerRegistrationService>(UriTemplates.CreateWorkerRegistrationUri(ServerUri));
            registration.Disconnect(LocalUri);
            ConnectionControl.Disconnect();
            TasksServiceHost.Close();
            PerformanceServiceHost.Close();
            Dispose();
        }
    }
}
