using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistributedComputing
{
    internal class ConnectionMeasuring
    {
        private Uri CheckedConnection;
        private int PauseInterval = 300;
        private bool EndMeasuring;
        public ConnectionMeasuring(Uri connection)
        {
            CheckedConnection = UriTemplates.CreateCheckConnectionUri(connection);
        }
        private void Pause()
        {
            Thread.Sleep(PauseInterval);
            EndMeasuring = true;
        }
        public int Measure()
        {
            var pauseThread = new Thread(Pause);
            EndMeasuring = false;
            var service = RemoteConnection.CreateProxy<ICheckConnection>(CheckedConnection);
            int result = 0;
            pauseThread.Start();
            while (!EndMeasuring)
            {
                result += service.Check();
            }
            return result;
        }
    }
}
