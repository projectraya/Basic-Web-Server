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
        private readonly Dictionary<Method, Dictionary<string, Func<Request, Response>>> _routes;

        public RoutingTable()
        {
            this._routes = new ()
            {
                [Method.GET] = new(),
                [Method.POST] = new(),
                [Method.PUT] = new(),
                [Method.DELETE] = new()
            };
        }
        public IRoutingTable Map(string path, Method method, Func<Request, Response> responseFunction)
        {
            Guard.AgainstNull(path, nameof(path));
            Guard.AgainstNull(responseFunction, nameof(responseFunction));

            this._routes[method][path] = responseFunction;

            return this;
            
        }

        public IRoutingTable MapGet(string path, Func<Request, Response> responseFunction)
        => Map(path, Method.GET, responseFunction);

        public IRoutingTable MapPost(string path, Func<Request, Response> responseFunction)
        => Map(path, Method.POST, responseFunction);

        public Response MatchRequest(Request request)
        {
            var requestMethod = request.Method;
            var requestURL = request.URL;

            if(!this._routes.ContainsKey(requestMethod)
                || !this._routes[requestMethod].ContainsKey(requestURL))
            {
                return new NotFoundResponse();
            }

            var responseFunction = this._routes[requestMethod][requestURL];
            return responseFunction(request);
        }
    }
}
