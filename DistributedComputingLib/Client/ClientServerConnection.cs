using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ServiceModel;

namespace DistributedComputing
{
    internal class ClientServerConnection : Connection
    {
        private ServiceHost DistributedTaskLibraryHost;
        private ConnectionControl ConnectionControl;
        public ClientServerConnection(IPAddress localIp,IPAddress serverIp,int serverPort) : base(localIp,serverIp,serverPort)
        {
            Logging.WriteLog("Connecting to server on " + ServerUri);
            DistributedTaskLibraryHost = RemoteConnection.CreateHost(UriTemplates.CreateLibrariesUri(LocalUri), typeof(IDistributedTaskLibraryService), typeof(DistributedTaskLibraryService));
            DistributedTaskLibraryHost.Open();
            try
            {
                var registration = RemoteConnection.CreateProxy<IClientRegistrationService>(UriTemplates.CreateClientRegistrationUri(ServerUri));
                registration.Connect(LocalUri);
            }
            catch(Exception e)
            {
                DistributedTaskLibraryHost.Close();
                Dispose();
                throw new NoServerConnectionException();
            }
            ConnectionControl = new ConnectionControl(this, ServerUri);
        }
        public ClientTask CreateTask(String typename,List<string>filenames)
        {
            if (Disposed)
            {
                throw new NoServerConnectionException();
            }
            else
            {
                var libraries = new DistributedTaskLibrary(LocalUri, filenames, typename);
                var service = RemoteConnection.CreateProxy<IServerTaskService>(UriTemplates.CreateTasksUri(ServerUri));
                int taskid = 0;
                try
                {
                    taskid = service.CreateTask(libraries);
                }
                catch
                {
                    throw new ServerUnavailableException();
                }
                return new ClientTask(taskid, ServerUri, LocalUri);
            }
        }
        public void Close()
        {
            var registration = RemoteConnection.CreateProxy<IClientRegistrationService>(UriTemplates.CreateClientRegistrationUri(ServerUri));
            registration.Disconnect(LocalUri);
            DistributedTaskLibraryHost.Close();
            ConnectionControl.Disconnect();
            Dispose();
        }
    }
}
