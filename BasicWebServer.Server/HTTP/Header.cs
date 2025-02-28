﻿using BasicWebServer.Server.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.HTTP
{
    public class Header
    {

        public const string ContentType = "Content-Type";
        public const string ContentDisposition = "Content-Disposition";
        public const string ContentLength = "Content-Length";
        public const string Date = "Date";
        public const string Location = "Location";
        public const string Server = "Server";
        public const string Cookie = "Cookie";
        public const string SetCookie = "Set-Cookie";
        public Header(string name, string value)
        {
            Guard.AgainstNull(name, nameof(name));
            Guard.AgainstNull(value, nameof(value));

            this.Name = name;
            this.Value = value;
            
        }

        public string Name { get; init; } //init means once its created, it cannot be changed
        public string Value { get; set; }

        public override string ToString()
        {
            //Content-Type: text/plain
            return $"{this.Name}: {this.Value}";
        }
    }
}
