using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DistributedComputing;
using IPDetectorLib;

namespace DistributedComputingServer
{
    class Program
    {
        static bool Hosted = false;
        static Server Server;

        static void Host()
        {
            IPAddress localIP = IPDetector.SelectIP();
            try
            {
                Server = new Server(localIP);
                Hosted = true;
                Console.WriteLine("Server is hosted on \"" + Server.ServerUri + "\"");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to host server on " + localIP.ToString());
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Distributed Computing Server");
            while (!Hosted)
            {
                Host();
            }
            Console.ReadLine();
            Server.Dispose();
        }
    }
}