using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace IPDetectorLib
{
    public static class IPDetector
    {
        public static List<IPAddress> GetIPList()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry("");
            List<IPAddress> list = Array.FindAll(hostEntry.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork).ToList();
            list.Add(new IPAddress(new byte[] { 127, 0, 0, 1 }));
            return list;
        }
        public static IPAddress SelectIP()
        {
            IPAddress result = null;
            IPHostEntry hostEntry = Dns.GetHostEntry("");
            List<IPAddress> list = Array.FindAll(hostEntry.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork).ToList();
            list.Add(new IPAddress(new byte[] { 127, 0, 0, 1 }));
            Console.WriteLine("Choose IP-address:");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine((i + 1).ToString() + ". " + list[i].ToString());
            }
            bool selected = false;
            do
            {
                try
                {
                    var select = int.Parse(Console.ReadLine());
                    result = list[select - 1];
                    selected = true;
                }
                catch
                {
                    Console.WriteLine("Input error,choose number from 1 to " + list.Count);
                }
            }
            while (!selected);
            return result;
        }
    }
}
