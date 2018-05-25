using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace DistributedComputing
{
    internal class ServerConnectionControl
    {
        private List<Uri> WorkerConnections;
        private List<Uri> ClientConnections;
        private Server Server;
        private int TimeOut = 1000;
        private Thread MainThread;
        public ServerConnectionControl(Server server)
        {
            Server = server;
            WorkerConnections = new List<Uri>();
            ClientConnections = new List<Uri>();
        }
        public void AddWorkerConnection(Uri connectionUri)
        {
            lock (WorkerConnections)
            {
                if (WorkerConnections.Count == 0)
                {
                    MainThread = new Thread(Control);
                    MainThread.Start();
                }
                WorkerConnections.Add(connectionUri);
            }
        }
        public void AddClientConnection(Uri connectionUri)
        {
            lock (ClientConnections)
            {
                if (ClientConnections.Count == 0)
                {
                    MainThread = new Thread(Control);
                    MainThread.Start();
                }
                ClientConnections.Add(connectionUri);
            }
        }
        public void Control()
        {
            List<Uri> removedClientConnections = new List<Uri>();
            List<Uri> removedWorkerConnections = new List<Uri>();
            while (WorkerConnections.Count!=0 && ClientConnections.Count != 0)
            {
                removedWorkerConnections.Clear();
                removedClientConnections.Clear();
                lock (WorkerConnections)
                {
                    foreach (Uri connection in WorkerConnections)
                    {
                        try
                        {
                            var service = RemoteConnection.CreateProxy<ICheckConnection>(UriTemplates.CreateCheckConnectionUri(connection));
                            service.Check();
                        }
                        catch
                        {
                            removedWorkerConnections.Add(connection);
                        }
                    }
                    lock (Server.Workers)
                    {
                        foreach (Uri connection in removedWorkerConnections)
                        {
                            WorkerConnections.Remove(connection);
                            Server.Workers.Remove(Server.Workers.Find(info => info.WorkerUri == connection));
                        }
                    }
                }
                lock (ClientConnections)
                {
                    foreach (Uri connection in ClientConnections)
                    {
                        try
                        {
                            var service = RemoteConnection.CreateProxy<ICheckConnection>(UriTemplates.CreateCheckConnectionUri(connection));
                            service.Check();
                        }
                        catch
                        {
                            removedClientConnections.Add(connection);
                        }
                    }
                    lock (Server.Clients)
                    {
                        foreach (Uri connection in removedClientConnections)
                        {
                            ClientConnections.Remove(connection);
                            Server.Clients.Remove(Server.Clients.Find(uri => uri == connection));
                        }
                        if (Server.Clients.Count == 0)
                        {
                            Server.CloseClientConnection();
                        }
                    }
                }
                Thread.Sleep(TimeOut);
            }
        }
        public void DeleteWorkerConnection(Uri connectionUri)
        {
            lock(WorkerConnections)
            {
                WorkerConnections.Remove(connectionUri);
            }
        }
        public void DeleteClientConnection(Uri connectionUri)
        {
            lock (ClientConnections)
            {
                ClientConnections.Remove(connectionUri);
            }
        }
    }
}
