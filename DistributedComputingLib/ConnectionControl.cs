using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DistributedComputing
{
    internal class ConnectionControl
    {
        private int TimeOut = 1000;
        private Thread MainThread;
        private IDisposable Connection;
        private Uri ServerUri;
        private bool Disconnected = false;
        private bool Connected = true;
        public ConnectionControl(IDisposable connection, Uri serverUri)
        {
            Connection = connection;
            ServerUri = serverUri;
            MainThread = new Thread(Control);
            MainThread.Start();
        }
        private void Control()
        {
            var service = RemoteConnection.CreateProxy<ICheckConnection>(UriTemplates.CreateCheckConnectionUri(ServerUri));
            while (Connected)
            {
                try
                {
                    service.Check();
                }
                catch
                {
                    Connected = false;
                }
                Thread.Sleep(TimeOut);
            }
            if (!Disconnected)
            {
                Logging.WriteLog("Connection lost");
                Connection.Dispose();
            }
        }
        public void Disconnect()
        {
            Disconnected = true;
            Connected = false;
            MainThread.Join();
        }
    }
}
