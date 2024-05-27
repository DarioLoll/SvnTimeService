using System;
using System.Net;
using System.Net.Sockets;
using SvnTimeService.Shared;

namespace SvnTimeService.Client.Core
{
    public class SvnTimeClient
    {
        private Socket _socket;

        private SocketReader _socketReader;
        private SocketWriter _socketWriter;
        
        
        public void Connect(IPAddress ip, int port)
        {
            //Wenn bereits verbunden, dann methode beenden
            if (_socket != null && _socket.Connected)
                return;
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socketReader = new SocketReader(_socket);
                _socketWriter = new SocketWriter(_socket);
                _socket.Connect(new IPEndPoint(ip, port));
            }
            catch (Exception)
            {
                Console.WriteLine("Could not connect to server.");
            }
        }

        public void Send(string msg)
        {
            _socketWriter.Write(msg);
        }

        public string Receive()
        {
            return _socketReader.Read();
        }
    }
}