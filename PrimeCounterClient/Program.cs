using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrimeNumbersCounterLib;
using System.Net;
using IPDetectorLib;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
namespace PrimeCounterClient
{
    class Program
    {
        private static bool Connected = false;
        private static PrimeNumbersCounter Counter;
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
                Counter = new PrimeNumbersCounter(localIP, serverIP, port);
                Connected = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to create client on " + localIP.ToString() + " or connect to server on " + serverIP.ToString() + ":" + port.ToString());
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to prime numbers counter");
            while (!Connected)
            {
                Connect();
            }
            List<int> testParams = null;
            using (FileStream stream = new FileStream("testnumbers.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<int>));
                testParams = (List<int>)serializer.Deserialize(stream);
            }
            Console.WriteLine("Start to count prime numbers");
            using (FileStream resstream = new FileStream("results.txt", FileMode.Create))
            {
                foreach (int num in testParams)
                {
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    Counter.CountPrimeNumbers(num);
                    watch.Stop();
                    byte[] res = Encoding.UTF8.GetBytes(num + " " + watch.ElapsedMilliseconds + "; ");
                    resstream.Write(res,0,res.Length);
                    Console.WriteLine("Time for count prime numbers to " + num + " is " + watch.ElapsedMilliseconds);
                }
            }
            Console.WriteLine("Press any key to exit...");
            Counter.Dispose();
            Console.ReadLine();
        }
    }
}
