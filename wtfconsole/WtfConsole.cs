using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace wtfconsole
{
    class WtfConsole
    {
        private const int listenPort = 9898;
        static void Main(string[] args)
        {
            Thread rx = new Thread(Recieve);
            Thread tx = new Thread(Send);
            rx.Start();
            tx.Start();
        }

        static void Recieve()
        {
            using (UdpClient listener = new UdpClient(listenPort))
            {
                try
                {
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
                    while (true)
                    {
                        byte[] bytes = listener.Receive(ref groupEP);
                        Console.WriteLine("<{0}> {1}\n",
                            groupEP.Address.ToString(),
                            Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error receiving: " + e.ToString());
                }
            }
        }

        static void Send()
        {
            using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                    ProtocolType.Udp))
            {
                try
                {
                    s.EnableBroadcast = true;
                    IPEndPoint ep = new IPEndPoint(IPAddress.Broadcast, 9898);
                    while (true)
                    {
                        byte[] sendbuf = Encoding.ASCII.GetBytes(Console.ReadLine());
                        s.SendTo(sendbuf, ep);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error sending: " + e.ToString());
                }
            }
        }
    }
}
