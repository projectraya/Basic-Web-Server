using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.HTTP
{
    public class Response
    {
        public Response(StatusCode statusCode)
        {
            this.Status = statusCode;

            this.Headers.Add(Header.Server, "My Web Server");
            this.Headers.Add(Header.Server, $"{DateTime.UtcNow:R}");
        }
        public StatusCode Status { get; init; }

        public HeaderCollection Headers { get; set; } = new HeaderCollection();

        public string Body { get; set; }
    }
}
