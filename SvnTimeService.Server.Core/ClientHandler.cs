using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using SvnTimeService.Server.Contracts;

namespace SvnTimeService.Server.Core
{
    public class ClientHandler
    {
        private Socket _socket;

        private IServiceLogger _logger;

        private List<Session> _sessions = new List<Session>();

        public ClientHandler(Socket socket, IServiceLogger logger)
        {
            _socket = socket;
            _logger = logger;
        }

        public void AcceptClients()
        {
            while (true)
            {
                try
                {
                    Socket clientSocket = _socket.Accept();
                    _logger.LogSystemInfo($"A client connected from {clientSocket.RemoteEndPoint}");
                    Session newSession = new Session(clientSocket, _logger);
                    _sessions.Add(newSession);
                    newSession.SessionClosed += OnSessionClosed;

                    Thread communicationThread = new Thread(newSession.HandleCommunication);
                    communicationThread.IsBackground = true;
                    communicationThread.Start();
                }
                catch (Exception e)
                {
                    _logger.LogSystemInfo("Stopped accepting clients.");
                    //Disconnecting all clients
                    //ToList() braucht man hier weil das eine Kopie von _sessions erstellt
                    //wenn wir durch die Liste _sessions gehen würden, würde hier eine Exception passieren
                    //weil während wir über die Liste in der foreach gehen, die sessions entfernt werden
                    //Man kann nicht über eine Liste gehen und gleichzeitig die Elemente der Liste ändern (Elemente hinzufügen oder entfernen)
                    foreach (var session in _sessions.ToList())
                    {
                        session.Close();
                    }

                    return;
                }
            }
        }

        private void OnSessionClosed(object sender, EventArgs e)
        {
            Session closedSession = (Session)sender;
            _sessions.Remove(closedSession);
        }
    }
}