using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.HTTP
{
    public class CookieCollection : IEnumerable<Cookie>
    {
        private readonly Dictionary<string, Cookie> _cookies = new Dictionary<string, Cookie>();

            
        public string this[string name] => this._cookies[name].Value;
        public void Add(string name, string value) => this._cookies[name] = new Cookie(name, value);

        public bool Contains(string name) => this._cookies.ContainsKey(name);
        public IEnumerator<Cookie> GetEnumerator()
        => this._cookies.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
    }
}

