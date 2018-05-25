using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DistributedComputing;

namespace PrimeNumbersCounterLib
{
    public class PrimeNumbersCounter : IDisposable
    {
        private TaskHandler<PrimeNumbersProgression, List<int>, PrimeNumbersDivider, PrimeNumbersCompiler, PrimeCountProcess> TaskHandler;
        private TaskHandlerFactory TaskHandlerFactory;
        private bool Disposed = false;
        public PrimeNumbersCounter(IPAddress localIP,IPAddress serverIP,int serverPort)
        {
            TaskHandlerFactory = new TaskHandlerFactory(localIP, serverIP, serverPort);
            TaskHandler = TaskHandlerFactory.CreateTaskHandler<PrimeNumbersProgression, List<int>, PrimeNumbersDivider, PrimeNumbersCompiler, PrimeCountProcess>();
        }
        public List<int> CountPrimeNumbers(int max)
        {
            return TaskHandler.Execute(new PrimeNumbersProgression(max));
        }
        public void Dispose()
        {
            TaskHandlerFactory.Dispose();
            Disposed = true;
        }
        ~PrimeNumbersCounter()
        {
            if (!Disposed)
            {
                Dispose();
            }
        }
    }
}
