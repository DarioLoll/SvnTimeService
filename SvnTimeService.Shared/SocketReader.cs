using System.Net.Sockets;
using System.Text;

namespace SvnTimeService.Shared
{
    public class SocketReader
    {
        private Socket _socket;

        public SocketReader(Socket socket)
        {
            _socket = socket;
        }

        public string Read()
        {
            byte[] buffer = new byte[1024];
            _socket.Receive(buffer);
            string text = Encoding.UTF8.GetString(buffer).Trim('\0');
            return text;
        }
    }
}