using BasicWebServer.Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace BasicWebServer.Server.HTTP
{
    public class Request
    {
        private static Dictionary<string, string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string, string>();

            if (headers.Contains(Header.ContentType)
                && headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {

                var parsedResult = ParseFormData(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }

            return formCollection;
        }

        private static Dictionary<string, string> ParseFormData(string bodyLines)
        => HttpUtility.UrlDecode(bodyLines)
            .Split('&')
            .Select(part => part.Split('='))
            .Where(part => part.Length == 2)
            .ToDictionary(part => part[0], part => part[1], StringComparer.InvariantCultureIgnoreCase);

        private static Method ParseMethod(string method)
        {
            try
            {
                return (Method)Enum.Parse(typeof(Method), method, true);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"Method '{method}' is not supported");
            }
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
        {
            var headerCollection = new HeaderCollection();

            foreach (var headerLine in headerLines)
            {
                if (headerLine == string.Empty)
                {
                    break;
                }

                var headerParts = headerLine.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid");
                }

                var headerName = headerParts[0];
                var headerValue = headerParts[1].Trim();

                headerCollection.Add(headerName, headerValue);
            }

            return headerCollection;
        }

        private static Dictionary<string, Session> Sessions = new();
        public Method Method { get; private set; }

        public string URL { get; set; }
        public HeaderCollection Headers { get; private set; }
        public CookieCollection Cookies { get; private set; }

        public string Body { get; private set; }
        public Session Session { get; private set; }
        
        public IReadOnlyDictionary<string, string> Form { get; private set; }

        public static Request Parse(string request)
        {
            var lines = request.Split(Environment.NewLine);
            var startLine = lines.First().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var method = ParseMethod(startLine[0]);
            var url = startLine[1];

            HeaderCollection headers = ParseHeaders(lines.Skip(1)); //we give every one to the method except for the first element
            CookieCollection cookies = ParseCookies(headers);
            Session session = GetSession(cookies);


            var bodyLines = lines.Skip(headers.Count + 2).ToArray(); //we need 2 more cuz of the empty line

            var body = string.Join(Environment.NewLine, bodyLines);
            var form = ParseForm(headers, body);

            return new Request
            {
                Method = method,
                URL = url,
                Headers = headers,
                Cookies = cookies,
                Body = body,
                Session = session,
                Form = form
            };
        }

        private static Session GetSession(CookieCollection cookies)
        {
            var sessionId = cookies.Contains(Session.SessionCookieName) ?
                cookies[Session.SessionCookieName] :
                Guid.NewGuid().ToString();

            if (!Sessions.ContainsKey(sessionId))
            {
                Sessions[sessionId] = new Session(sessionId);
            }

            return Sessions[sessionId];
        }

        public static CookieCollection ParseCookies(HeaderCollection headers)
        {
            var cookieCollection = new CookieCollection();
            if (headers.Contains(Header.Cookie))
            {
                var cookieHeader = headers[Header.Cookie];
                var allCookies = cookieHeader.Split(';');

                foreach(var cookieText in allCookies)
                {
                    var cookieParts = cookieText.Split('=');
                    var cookieName = cookieParts[0].Trim();
                    var cookieValue = cookieParts[1].Trim();

                    cookieCollection.Add(cookieName, cookieValue);
                }
            }

            return cookieCollection;
        }
        
    }
}
