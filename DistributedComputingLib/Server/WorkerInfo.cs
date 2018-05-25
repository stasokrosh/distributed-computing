using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    internal class WorkerInfo
    {
        public Uri WorkerUri{ get; private set; }
        public List<ConnectionInfo> Connections;
        public Dictionary<int,int> Tasks;
        public WorkerInfo(Uri workerUri)
        {
            WorkerUri = workerUri;
            Connections = new List<ConnectionInfo>();
            Tasks = new Dictionary<int, int>();
        }
        public int GetPerformance(Uri ClientUri)
        {
            var service = RemoteConnection.CreateProxy<IWorkerPerformanceService>(UriTemplates.CreatePerformanceUri(WorkerUri));
            int machinePerformance = service.GetPerformance();
            return machinePerformance / Connections.Where(connection => connection.ClientUri == ClientUri).First().Connection;
        }
    }
}
