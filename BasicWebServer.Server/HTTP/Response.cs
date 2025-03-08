﻿using System;
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
            this.StatusCode = statusCode;

            this.Headers.Add(Header.Server, "My Web Server");
            this.Headers.Add(Header.Date, $"{DateTime.UtcNow:R}");
        }
        public StatusCode StatusCode { get; init; }

        public HeaderCollection Headers { get; } = new HeaderCollection();
        public CookieCollection Cookies { get; } = new CookieCollection();

        public string Body { get; set; }
       

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.StatusCode}");

            foreach(var header in this.Headers) //this is possible thanks to ienumerator
            {
                result.AppendLine(header.ToString());
            }

            foreach (var cookie in this.Cookies)
            {
                result.AppendLine($"{Header.SetCookie}: {cookie}");
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
