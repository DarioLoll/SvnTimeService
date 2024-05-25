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
        
        public const string DefaultIp = "127.0.0.1";
        public const int DefaultPort = 4004;
        
        public bool Connect(string ipAddress = DefaultIp, int port = DefaultPort)
        {
            if (_socket != null && _socket.Connected)
                return true;
            if (!IPAddress.TryParse(ipAddress, out IPAddress ip))
            {
                ip = IPAddress.Parse(DefaultIp);
            }

            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socketReader = new SocketReader(_socket);
                _socketWriter = new SocketWriter(_socket);
                _socket.Connect(new IPEndPoint(ip, port));
                return true;
            }
            catch (Exception)
            {
                return false;
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

        public void Close()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
    }
}