using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace windows_network_sniffer
{
    class sniffer
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sniffer de Rede");
            Console.WriteLine("+++++++++++++++");

            var ipAddress = SelectIPAddress();

            Console.WriteLine("IP: " + ipAddress);

            var socket = CreateSocket(ipAddress);


            var buffer = new byte[65535];
            var bufferLength = socket.Receive(buffer);

            Console.WriteLine("Bytes" + bufferLength);
            Console.WriteLine(Encoding.Default.GetString(buffer));
        }

        private static object CreateSocket(IPAddress ipAdress)
        {
            var socket = new Socket(
                ipAdress.AddressFamily,
                SocketType.Raw,
                ProtocolType.IP);

            socket.Bind(new IPEndPoint(ipAdress, 0));

            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            var optionIn = new byte[] {1,2,3,4};

            socket.IOControl(
                IOControlCode.ReceiveAll,
                optionIn,
                new byte[4]);

            return socket;
        }

        static IPAddress SelectIPAddress()
        {
            var hostname = Dns.GetHostName();
            Console.WriteLine("Computador: " + hostname);

            var hostnameEntry = Dns.GetHostEntry(hostname);
            var ipAddresses = hostnameEntry.AddressList;

            Console.WriteLine("Endereços IP:");
            for (var i = 0; i < ipAddresses.Length; i++)
            {
                var ipAddress = ipAddresses[i];
                Console.WriteLine("  " + (i + 1) + ") " + ipAddress);
            }

            int selectedIndex;
            do
            {
                Console.Write("Selecione: ");
            } while (int.TryParse(Console.ReadLine(), out selectedIndex) == false ||
                     selectedIndex < 1 ||
                     selectedIndex > ipAddresses.Length);

            return ipAddresses[selectedIndex - 1];
        }
    }
}
