using System;
using System.Net.Sockets;
using SvnTimeService.Server.Contracts;
using SvnTimeService.Shared;

namespace SvnTimeService.Server.Core
{
    public class Session
    {
        private Socket _clientSocket;

        private SocketReader _socketReader;
        private SocketWriter _socketWriter;

        private IServiceLogger _logger;

        public event EventHandler SessionClosed; 

        public Session(Socket clientSocket, IServiceLogger logger)
        {
            _clientSocket = clientSocket;
            _logger = logger;
            _socketReader = new SocketReader(_clientSocket);
            _socketWriter = new SocketWriter(_clientSocket);
        }

        public void HandleCommunication()
        {
            while (true)
            {
                try
                {
                    string request = _socketReader.Read();
                    _logger.LogRequestInfo($"Received request: \" {request} \" from client on {_clientSocket.RemoteEndPoint}");
                    if (request == "shutdown")
                    {
                        Close();
                        return;
                    }
                    SvnTimeRequestHandler requestHandler = new SvnTimeRequestHandler(request);
                    string response = requestHandler.GetResponse();
                    _socketWriter.Write(response);
                }
                catch (Exception)
                {
                    Close();
                }
            }
        }

        public void Close()
        {
            if (_clientSocket.Connected)
            {
                _logger.LogSystemInfo($"Client disconnected from {_clientSocket.RemoteEndPoint}");
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();
                OnSessionClosed();
            }
        }

        protected virtual void OnSessionClosed()
        {
            SessionClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}