using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class ServerTaskService : IServerTaskService
    {
        private Server Server;
        private IDistributingStrategy DistributingStrategy;
        private int TryCount = 3;
        public ServerTaskService(Server server)
        {
            Server = server;
            DistributingStrategy = new DistributingStrategy();
        }
        public int CreateTask(DistributedTaskLibrary libraries)
        {
            Logging.WriteLog("Request for creating new library was received");
            int taskId = Server.TaskLibraryManager.CreateTask(libraries);
            DistributedTaskLibrary distributedLibrary = Server.TaskLibraryManager.Tasks[taskId].CreateDistributedTaskLibrary(Server.ServerUri);
            List<int> list = new List<int>();
            lock (Server.Workers)
            {
                foreach (WorkerInfo workerInfo in Server.Workers)
                {
                    if (!workerInfo.Tasks.Keys.ToList().Contains(taskId))
                    {
                        var service = RemoteConnection.CreateProxy<IWorkerTaskService>(UriTemplates.CreateTasksUri(workerInfo.WorkerUri));
                        workerInfo.Tasks.Add(taskId, service.CreateTask(distributedLibrary));
                    }
                }
            }
            return taskId;
        }
        public List<DistributedComputingResult> ExecuteTask(Uri clientUri, int taskId, DividedResource resource)
        {
            Logging.WriteLog("Request for executing task  was received");
            if (Server.Workers.Count == 0)
            {
                return null;
            }
            List<TaskExecutor> executors = DistributingStrategy.Distribute(clientUri, resource.Resources, Server.Workers);
            var task = ExecuteTaskAsync(clientUri, taskId, executors, resource.Resources.Count);
            task.Wait();
            var result = task.Result;
            return result;
        }
        public async Task<List<DistributedComputingResult>> ExecuteTaskAsync(Uri clientUri, int taskId, List<TaskExecutor> executors, int partsCount)
        {
            var tasks = new Dictionary<Task<DistributedResource>, int>();
            var results = new DistributedComputingResult[partsCount];
            foreach (TaskExecutor executor in executors)
            {
                foreach (TaskPart taskPart in executor.Tasks)
                {
                    results[taskPart.Position] = new DistributedComputingResult() { Data = taskPart.Resource };
                    tasks.Add(ExecuteAsync(clientUri, executor.WorkerInfo.WorkerUri, taskPart.Resource, executor.WorkerInfo.Tasks[taskId]), taskPart.Position);
                }
            }
            await Task.WhenAll(tasks.Keys.ToArray());
            foreach (Task<DistributedResource> task in tasks.Keys.ToArray())
            {
                results[tasks[task]].Result = task.Result;
                if (task.Result == null)
                {
                    return null;
                }
            }
            return results.ToList();
        }
        private async Task<DistributedResource> ExecuteAsync(Uri clientUri, Uri workerUri, DistributedResource resource, int taskId)
        {
            return await Task.Run(() =>
            {
                DistributedResource result = null;
                try
                {
                    var service = RemoteConnection.CreateProxy<IWorkerTaskService>(UriTemplates.CreateTasksUri(workerUri));
                    result = service.ExecuteTask(taskId, resource);
                }
                catch
                {
                    result = null;
                }
                result = Execute(clientUri, taskId, resource);
                return result;
            });
        }
        public DistributedResource Execute(Uri clientUri, int taskId, DistributedResource resource)
        {
            Logging.WriteLog("Request for executing task on single resource was received");
            DistributedResource result = null;
            int trycount = TryCount;
            do
            {
                WorkerInfo workerInfo = null;
                lock (Server.Workers)
                {
                    List<int> performance = Server.Workers.Select<WorkerInfo, int>(worker => worker.GetPerformance(clientUri)).ToList();
                    if (performance.Count != 0)
                    {
                        workerInfo = Server.Workers[performance.FindIndex(perform => perform == performance.Max())];
                        var service = RemoteConnection.CreateProxy<IWorkerTaskService>(UriTemplates.CreateTasksUri(workerInfo.WorkerUri));
                        try
                        {
                            result = service.ExecuteTask( workerInfo.Tasks[taskId], resource);
                            trycount--;
                        }
                        catch
                        { }
                    }
                }
            }
            while (result == null && Server.Workers.Count != 0 && trycount > 0) ;
            return result;
        }
    }
}
