using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DistributedComputing
{
    public class TaskHandlerFactory : IDisposable
    {
        internal ClientServerConnection ServerConnection;
        private bool Disposed = false; 
        public TaskHandlerFactory(IPAddress localIp,IPAddress serverIp,int serverPort)
        {
            ServerConnection = new ClientServerConnection(localIp, serverIp, serverPort);
        }
        public TaskHandler<T, V, Divider, Compiler, Process> CreateTaskHandler<T, V, Divider, Compiler, Process>()
            where Divider : IObjectDivider<T>, new()
            where Compiler : IResultCompiler<V>, new()
            where Process : IProcess<T,V>, new()
        {
            if (Disposed)
            {
                throw new ObjectDisposedException(ServerConnection.ToString());
            }
            else
            {
                try
                {
                    return new TaskHandler<T, V, Divider, Compiler, Process>(this);
                }
                catch (ObjectDisposedException)
                {
                    throw new NoServerConnectionException();
                }

            }
        }
        public void Dispose()
        {
            ServerConnection.Close();
            Disposed = true;
        }
    }
}
