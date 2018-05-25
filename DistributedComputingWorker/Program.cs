using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using DistributedComputing;
using IPDetectorLib;

namespace DistributedComputingWorker
{
    class Program
    {
        static bool Connected = false;
        static Worker Worker;
        
        static void Connect()
        {
            IPAddress localIP = IPDetector.SelectIP();
            IPAddress serverIP = null;
            bool entered = false;
            do
            {
                Console.WriteLine("Enter server IP-address");
                try
                {
                    serverIP = IPAddress.Parse(Console.ReadLine());
                    entered = true;
                }
                catch
                {
                    Console.WriteLine("Input error. Wrong IP-address format");
                }
            }
            while (!entered);
            int port = 0; 
            entered = false;
            do
            {
                Console.WriteLine("Enter server port");
                try
                {
                    port = int.Parse(Console.ReadLine());
                    entered = true;
                }
                catch
                {
                    Console.WriteLine("Input error. Port is integer number");
                }
            }
            while (!entered);
            try
            {
                Worker = new Worker(localIP, serverIP, port);
                Connected = true;
                Console.WriteLine("Worker is hosted on \"" + Worker.LocalUri + "\"");
                Console.WriteLine("Worker is connected to server on \"" + Worker.ServerUri + "\"");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to create worker on " + localIP.ToString() + " or connect to server on " + serverIP.ToString() + ":" + port.ToString());
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Distributed Computing Worker");
            while (!Connected)
            {
                Connect();
            }
            Console.ReadLine();
            Worker.Dispose();
        }
    }
}
