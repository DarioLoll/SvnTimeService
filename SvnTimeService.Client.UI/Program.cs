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
            bool connected = false;
            if (args.Length >= 1)
            {
                if (args.Length >= 2 && int.TryParse(args[1], out port))
                {
                    connected = _client.Connect(args[0], port);
                }
                else
                {
                    connected = _client.Connect(args[0]);
                }
            }
            else
            {
                connected = _client.Connect();
            }

            if (!connected)
            {
                Console.WriteLine("Failed to connect");
                return;
            }
            else
            {
                string welcomeMsg = _client.Receive();
                Console.WriteLine(welcomeMsg);
            }
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