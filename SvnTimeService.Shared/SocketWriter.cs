using System.Net.Sockets;
using System.Text;

namespace SvnTimeService.Shared
{
    public class SocketWriter
    {
        private Socket _socket;

        public SocketWriter(Socket socket)
        {
            _socket = socket;
        }

        public void Write(string msg)
        {
            _socket.Send(Encoding.UTF8.GetBytes(msg));
        }
    }
}