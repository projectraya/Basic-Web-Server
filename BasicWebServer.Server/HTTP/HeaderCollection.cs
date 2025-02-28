using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.HTTP
{
    public class HeaderCollection : IEnumerable<Header>
    {
        private readonly Dictionary<string, Header> _headers;

        public HeaderCollection()
        {
            this._headers = new Dictionary<string, Header>();
        }

        public string this[string name] => this._headers[name].Value;

        public int Count  => this._headers.Count;

        public bool Contains(string name) => this._headers.ContainsKey(name);

        public void Add(string name, string value)

        => this._headers[name] = new Header(name, value);

        public IEnumerator<Header> GetEnumerator()
        {
            return this._headers.Values.GetEnumerator(); //because its a dictionary, we only want the headers(values)
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator(); //same as return GetEnumerator();

    }
}
