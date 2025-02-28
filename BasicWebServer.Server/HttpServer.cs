﻿using BasicWebServer.Server.HTTP;
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
        public async Task StartAsync()
        {
            this._serverListener.Start();

            Console.WriteLine($"Server started on port: {_port}");
            Console.WriteLine($"Listening for requests...");


            while (true)
            {
                var connection = await _serverListener.AcceptTcpClientAsync();

                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();

                    var requestText = await this.ReadRequestAsync(networkStream);

                    Console.WriteLine(requestText);

                    Request request = Request.Parse(requestText);
                    Response response = this._routingTable.MatchRequest(request);

                    //execute pre render action for the response
                    if (response.PreRenderAction != null)
                    {
                        response.PreRenderAction(request, response);
                    }

                    AddSession(request, response);

                    await WriteResponseAsync(networkStream, response);

                    connection.Close();
                });
                
            }
        }

        private void AddSession(Request request, Response response)
        {
            var sessionExists = request.Session.ContainsKey(Session.SessionCurrentDateKey);

            if (!sessionExists)
            {
                request.Session[Session.SessionCurrentDateKey] = DateTime.Now.ToString();
                response.Cookies.Add(Session.SessionCookieName, request.Session.Id);
            }
        }

        private async Task WriteResponseAsync(NetworkStream networkStream, Response response)
        {
            //here we had things hardcoded, which we implemented implicitly in ContentResponse
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            await networkStream.WriteAsync(responseBytes);
        }

        private async Task<string> ReadRequestAsync(NetworkStream networkStream)
        {
            var bufferLength = 1024;
            var buffer = new Byte[bufferLength];

            var totalBytes = 0;
            var requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = await networkStream
                    .ReadAsync(buffer, 0, bufferLength);

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
