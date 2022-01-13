using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BasicWebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        public HttpServer(string ipAddress, int port)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListener = new TcpListener(this.ipAddress, this.port);
        }

        public void Start()
        {
            this.serverListener.Start();

            Console.WriteLine($"Server started on port {port}.");
            Console.WriteLine("Listening for requests...");

            while (true)
            {
                TcpClient connection = this.serverListener.AcceptTcpClient();

                NetworkStream networkStream = connection.GetStream();

                string content = "Hello from the server!";
                
                this.WriteResponse(networkStream, content);

                connection.Close();
            }
        }

        private void WriteResponse(NetworkStream networkStream, string message)
        {
            int contentLength = Encoding.UTF8.GetByteCount(message);

            string responseString = $@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=UTF-8
Content-Length: {contentLength}

{message}";

            byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);

            networkStream.Write(responseBytes);
        }
    }
}
