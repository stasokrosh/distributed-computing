using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    internal class ClientTask
    {
        private int Id;
        private Uri Server;
        private Uri Client;
        public ClientTask(int id,Uri server,Uri client)
        {
            Id = id;
            Client = client;
            Server = UriTemplates.CreateTasksUri(server);
        }
        public List<DistributedComputingResult> ExecuteTask(DividedResource resource)
        {
            try
            {
                var service = RemoteConnection.CreateProxy<IServerTaskService>(Server);
                return service.ExecuteTask(Client, Id, resource);
            }
            catch
            {
                throw new NoServerConnectionException();
            }
        } 
        public DistributedResource Execute(DistributedResource resource)
        {
            try
            {
                var service = RemoteConnection.CreateProxy<IServerTaskService>(Server);
                return service.Execute(Client, Id, resource);
            }
            catch
            {
                throw new NoServerConnectionException();
            }
        }
    }
}
