using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedComputing
{
    public class ConnectionInfo
    {
        public Uri ClientUri { get; private set; }
        public int Connection { get; private set; }
        public ConnectionInfo(Uri clientUri,int connection)
        {
            ClientUri = clientUri;
            Connection = connection;
        }
    }
}
