using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    internal class ConsoleLogger : ILogger
    {
        public void WriteLog(string log)
        {
            Console.WriteLine(log);
        }
    }
}
