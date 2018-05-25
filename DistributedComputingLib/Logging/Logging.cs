using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    public static class Logging
    {
        public static ILogger Logger = new ConsoleLogger();
        public static void WriteLog(string message)
        {
            Logger.WriteLog(message);
        }
    }
}
