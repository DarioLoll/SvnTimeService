using System;
using System.Net;
using SvnTimeService.Client.Core;

namespace SvnTimeService.Client.UI
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IPAddress ip;
            int port;
            SvnTimeClient _client = new SvnTimeClient();
            
            //Extrahieren von IP und Port aus args und connecten
            if (args.Length >= 1)
            {
                ip = IPAddress.Parse(args[0]);
                if (args.Length >= 2)
                {
                    port = int.Parse(args[1]);
                    _client.Connect(ip, port);
                }
                else
                {
                    _client.Connect(ip, 4004);
                }
            }
            else
            {
                _client.Connect(IPAddress.Loopback, 4004);
            }
            
            //Kommunikation mit dem Server
            while (true)
            {
                try
                {
                    Console.Write("Enter a command: ");
                    string request = Console.ReadLine();
                    _client.Send(request);
                    if (request == "shutdown")
                    {
                        break;
                    }
                    string response = _client.Receive();
                    Console.WriteLine(response);
                }
                catch (Exception e)
                {
                    break;
                }
                
            }

            Console.WriteLine("Disconnected");
            Console.ReadKey();
        }
    }
}