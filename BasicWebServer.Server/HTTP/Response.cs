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

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"HTTP/1.1 {(int)this.Status} {this.Status}");

            foreach(var item in this.Headers) //this is possible thanks to ienumerator
            {
                result.AppendLine(item.ToString());
            }

            result.AppendLine();

            if (!string.IsNullOrEmpty(this.Body)) //if there is something
            {
                result.Append(this.Body);
            }

            return result.ToString();
        }
    }
}
