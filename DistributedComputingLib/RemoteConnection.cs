using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net;
using System.Net.Sockets;

namespace DistributedComputing
{
    internal static class RemoteConnection
    {
        private static long MaxSize = 1000000000;
        public static T CreateProxy<T>(Uri holderUri)
        {
            var factory = new ChannelFactory<T>(new NetTcpBinding(SecurityMode.None)
            {
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.MaxValue,
                MaxReceivedMessageSize = MaxSize
            }, new EndpointAddress(holderUri));
            return factory.CreateChannel();
        }
        public static Uri CreateUri(IPAddress ip,int port)
        {
            return new Uri("net.tcp://" + ip + ":" + port.ToString() + "/");
        }
        public static ServiceHost CreateHost(Uri hostUri,Type interfaceType,Type holderType) 
        {
            var serviceHost = new ServiceHost(holderType,hostUri);
            serviceHost.AddServiceEndpoint(interfaceType,new NetTcpBinding(SecurityMode.None) { ReceiveTimeout = TimeSpan.MaxValue, SendTimeout = TimeSpan.MaxValue}, "");
            return serviceHost;
        }
        public static ServiceHost CreateHost(Uri hostUri, Type interfaceType,object singleton)
        {
            var serviceHost = new ServiceHost(singleton, hostUri);
            serviceHost.AddServiceEndpoint(interfaceType, new NetTcpBinding(SecurityMode.None) { ReceiveTimeout = TimeSpan.MaxValue, SendTimeout = TimeSpan.MaxValue}, "");
            return serviceHost;
        }
    }
}
