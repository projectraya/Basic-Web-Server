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

        public int Count  => this._headers.Count;

        public void Add(string name, string value)
        {
            Header header = new Header(name, value);
            this._headers.Add(name, header);
        }

        public IEnumerator<Header> GetEnumerator()
        {
            return this._headers.Values.GetEnumerator(); //because its a dictionary, we only want the headers(values)
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator(); //same as return GetEnumerator();

    }
}
