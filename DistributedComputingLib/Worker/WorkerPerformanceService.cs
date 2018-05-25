using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.ServiceModel;

namespace DistributedComputing
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class WorkerPerformanceService : IWorkerPerformanceService,IDisposable
    {
        private float Processor;
        private float Memory;
        private bool Working;
        private int Interval = 1000;
        private PerformanceCounter PCounter;
        private PerformanceCounter MCounter;
        private bool Disposed = false;
        public WorkerPerformanceService()
        {
            PCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            MCounter = new PerformanceCounter("Memory", "Available MBytes");
            Working = true;
            var thread = new Thread(GetValues);
            thread.Start();
        }
        private void GetValues()
        {
            while (Working)
            {
                Processor = PCounter.NextValue();
                Memory = MCounter.NextValue();
                Thread.Sleep(Interval);
            }
        }
        public int GetPerformance()
        {
            return (int)(Memory / Processor);
        }
        public void Dispose()
        {
            Working = false;
            Disposed = true;
        }
        ~WorkerPerformanceService()
        {
            if (!Disposed)
            {
                Dispose();
            }
        }
    }
}
