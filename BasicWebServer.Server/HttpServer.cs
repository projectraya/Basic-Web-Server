using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Routing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace BasicWebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress _ipAdress;
        private readonly int _port;
        private readonly TcpListener _serverListener;

        private readonly RoutingTable _routingTable;

        public HttpServer(string ipAdress, int port, Action<IRoutingTable> routingTableConfiguration)
        {
            this._ipAdress = IPAddress.Parse(ipAdress);
            this._port = port;

            this._serverListener = new TcpListener(_ipAdress, _port);

            routingTableConfiguration(this._routingTable = new RoutingTable()); //what is action?
        }

        public HttpServer(int port, Action<IRoutingTable> routingTable) : this("127.0.0.1", port, routingTable)
        {
            
        }

        public HttpServer(Action<IRoutingTable> routingTable) : this(8080, routingTable) //giving to the upper one
        {
            
        }
        public void Start()
        {
            _serverListener.Start();

            Console.WriteLine($"Server started on port: {_port}");
            Console.WriteLine($"Listening for requests...");

            while(true){

                var connection = _serverListener.AcceptTcpClient();

                var networkStream = connection.GetStream();

                var requestText = this.ReadRequest(networkStream);

                Console.WriteLine(requestText);

                Request request = Request.Parse(requestText);
                Response response = this._routingTable.MatchRequest(request);

                WriteResponse(networkStream, response);

                connection.Close();
            }

            
        }

        private void WriteResponse(NetworkStream networkStream, string message)
        {
            var contentLength = Encoding.UTF8.GetByteCount(message);

            var response = $@"HTTP/1.1 200 OK
Content-Type: text/plain; charset=UTF-8
Content-Length: {contentLength}

{message}";

            var responseBytes = Encoding.UTF8.GetBytes(response);

            networkStream.Write(responseBytes);
        }

        private string ReadRequest(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new Byte[bufferLength];

            var totalBytes = 0;
            var requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = networkStream.Read(buffer, 0, bufferLength);

                totalBytes += bytesRead;

                if(totalBytes > 1024 * 10)
                {
                    throw new InvalidOperationException("Request is too large");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bufferLength));
            }
            //May not run correctly over Internet
            while (networkStream.DataAvailable);

            return requestBuilder.ToString();
            
        }
    }
}
