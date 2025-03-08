using BasicWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.Routing
{
    public interface IRoutingTable
    {
        IRoutingTable Map(string path, Method method, 
            Func<Request, Response> responseFunction);

        IRoutingTable MapGet(string path, Func<Request, Response> responseFunction);
        // request => response
        // Controller / Action() => View()
        IRoutingTable MapPost(string path, Func<Request, Response> responseFunction);
        
    }
}
