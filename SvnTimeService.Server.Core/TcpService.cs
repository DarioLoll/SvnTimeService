using System.Net;
using System.Net.Sockets;
using System.Threading;
using SvnTimeService.Server.Contracts;

namespace SvnTimeService.Server.Core
{
    public class TcpService
    {
        private Socket _socket;

        private IServiceLogger _logger;

        private ClientHandler _clientHandler;

        public bool IsOnline { get; private set; }

        public TcpService(IServiceLogger logger)
        {
            _logger = logger;
        }

        public void Start(IPAddress ipAddress, int port)
        {
            if (_socket != null && IsOnline)
                return; //Wenn der Server schon gestartet ist, endet die Methode hier

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clientHandler = new ClientHandler(_socket, _logger);
            _socket.Bind(new IPEndPoint(ipAddress, port));
            _socket.Listen(20);
            _logger.LogSystemInfo($"Server started on {_socket.LocalEndPoint}.");
            IsOnline = true;

            Thread acceptingThread = new Thread(_clientHandler.AcceptClients);
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }

        public void Stop()
        {
            //Der Server wird nur gestoppt, wenn er bereits rennt.
            if (_socket != null && IsOnline)
            {
                _socket.Close();
                IsOnline = false;
                _logger.LogSystemInfo($"Server stopped.");
            }
        }
    }
}