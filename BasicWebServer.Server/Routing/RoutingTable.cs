using BasicWebServer.Server.Common;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.Routing
{
    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method, Dictionary<string, Response>> _routes;

        public RoutingTable()
        {
            this._routes = new Dictionary<Method, Dictionary<string, Response>>()
            {
                [Method.GET] = new Dictionary<string, Response>(),
                [Method.POST] = new Dictionary<string, Response>(),
                [Method.PUT] = new Dictionary<string, Response>(),
                [Method.DELETE] = new Dictionary<string, Response>()
            };
        }
        public IRoutingTable Map(string URL, Method method, Response response)
        {
            switch (method)
            {
                case Method.GET:

                    this.MapGet(URL, response);
                    break;

                case Method.POST:

                    this.MapPost(URL, response);
                    break;

                default:
                    throw new InvalidOperationException($"Method '{method}' is not supported.");

            }
            return this;
        }

        public IRoutingTable MapGet(string URL, Response response)
        {
            Guard.AgainstNull(URL, nameof(URL));
            Guard.AgainstNull(response, nameof(response));

            this._routes[Method.GET][URL] = response; //the value of the inside dictionary

            return this; //to be able to link with .MapGet().MapGet()
            //Linq works the same way
        }

        public IRoutingTable MapPost(string URL, Response response)
        {
            Guard.AgainstNull(URL, nameof(URL));
            Guard.AgainstNull(response, nameof(response));

            this._routes[Method.POST][URL] = response; //the value of the inside dictionary

            return this; //to be able to link with .MapGet().MapGet()
        }

        public Response MatchRequest(Request request)
        {
            var requestMethod = request.Method;
            var requestURL = request.URL;

            if(!this._routes.ContainsKey(requestMethod)
                || !this._routes[requestMethod].ContainsKey(requestURL))
            {
                return new NotFoundResponse();
            }

            return this._routes[requestMethod][requestURL];
        }
    }
}
