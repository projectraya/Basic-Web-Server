using BasicWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicWebServer.Server.Responses
{
    public class HTMLResponse : ContentResponse
    {
        public HTMLResponse(string text, 
            Action<Request, Response> preRenderAction = null)
            : base(text, ContentType.Html, preRenderAction)
        {

        }
    }
}
