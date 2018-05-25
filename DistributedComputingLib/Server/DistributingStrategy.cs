using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    internal class DistributingStrategy : IDistributingStrategy
    {
        internal class StaticWorkerInfo
        {
            internal WorkerInfo WorkerInfo;
            internal int Performance;
        }
        public List<TaskExecutor> Distribute(Uri clientUri, List<DistributedResource> resources, List<WorkerInfo> workers)
        {
            var result = new List<TaskExecutor>();
            lock (workers)
            {
                List<StaticWorkerInfo> staticWorkers = workers.Select<WorkerInfo, StaticWorkerInfo>
                    (workerInfo => new StaticWorkerInfo() { WorkerInfo = workerInfo,Performance = workerInfo.GetPerformance(clientUri)})
                    .OrderBy(staticWorkerInfo => staticWorkerInfo.Performance).ToList();
                int workerNum = 0;
                for(int i=0;i<resources.Count; i++)
                {
                    if (result.Count < staticWorkers.Count)
                    {
                        result.Add(new TaskExecutor() {WorkerInfo = staticWorkers[workerNum].WorkerInfo });
                    }
                    result[workerNum].Tasks.Add(new TaskPart() { Resource = resources[i], Position = i });
                    workerNum = (workerNum + 1) % staticWorkers.Count;
                }
            }
            return result;
        }
    }
}
