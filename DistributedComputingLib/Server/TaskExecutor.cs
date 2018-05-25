using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    internal class TaskExecutor
    {
        public WorkerInfo WorkerInfo;
        public List<TaskPart> Tasks;
        internal TaskExecutor()
        {
            Tasks = new List<TaskPart>();
        }

    }
}
