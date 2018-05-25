using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class WorkerTaskService : IWorkerTaskService,IDisposable
    {
        private bool Disposed = false;
        private DistributedResourceManager ResourceManager;
        private TaskLibraryManager TaskManager;
        private Uri LocalUri;
        public WorkerTaskService(Uri localUri)
        {
            ResourceManager = new DistributedResourceManager(localUri);
            TaskManager = new TaskLibraryManager();
            TaskManager.Load();
            LocalUri = localUri;
        }
        public int CreateTask(DistributedTaskLibrary libraries)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
            else
            {
                Logging.WriteLog("Request for creating new library was received");
                return TaskManager.CreateTask(libraries);
            }
        }
        public DistributedResource ExecuteTask(int taskId, DistributedResource resource)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
            else
            {
                try
                {
                    Logging.WriteLog("Request for executing task was received");
                    TaskLibrary task = TaskManager.Tasks[taskId];
                    var stream = new MemoryStream(resource.Get());
                    var serializer = new DataContractSerializer(task.InputData);
                    object input = null;
                    input = serializer.ReadObject(stream);
                    var process = Activator.CreateInstance(task.Process);
                    MethodInfo method = task.Process.GetMethod("Execute");
                    object executeResult = method.Invoke(process, new object[] { input });
                    DistributedResource result = null;
                    lock (ResourceManager)
                    {
                        result = ResourceManager.CreateDistributedResource(executeResult);
                    }
                    return result;
                }
                catch
                {
                    return null;
                }
            }
        }
        public int CheckConnection(Uri clientUri)
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
            else
            {
                var connection = new ConnectionMeasuring(clientUri);
                return connection.Measure();
            }
        }
        public void Dispose()
        {
            ResourceManager.Dispose();
            TaskManager.Save();
            Disposed = true;
        }
        ~WorkerTaskService()
        {
            if(!Disposed)
            {
                Dispose();
            }
        }
    }
}
