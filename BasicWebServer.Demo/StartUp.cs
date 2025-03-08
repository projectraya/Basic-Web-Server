using BasicWebServer.Demo.Controllers;
using BasicWebServer.Server;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using BasicWebServer.Server.Routing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace BasicWebServer.Demo
{
    public class StartUp
    {

        public async static Task Main()
        {


            HttpServer server = new HttpServer(routes => routes
            .MapGet<HomeController>("/", c => c.Index())
            .MapGet<HomeController>("/Redirect", c => c.Redirect())
            .MapGet<HomeController>("/HTML", c => c.Html())
            .MapPost<HomeController>("/HTML", c => c.HtmlFormPost())
            .MapGet<HomeController>("/Content", c => c.Content())
            .MapPost<HomeController>("/Content", c => c.DownloadContent())
            .MapGet<HomeController>("/Cookies", c => c.Cookies())
            .MapGet<HomeController>("/Session", c => c.Session())
            .MapGet<UserController>("/Login", c => c.Login())
            .MapPost<UserController>("/Login", c => c.LoginUser())
            .MapGet<UserController>("/Logout", c => c.Logout())
            .MapGet<UserController>("/UserProfile", c => c.GetUserData()));


            await server.StartAsync();
        }

        

        //private static void GetUserDataAction(Request request, Response response)
        //{
        //    if (request.Session.ContainsKey(Session.SessionUserKey))
        //    {
        //        response.Body = "";
        //        response.Body += $"<h3>Currently logged-in user " + $"is with username '{Username}'</h3>";
        //    }
        //    else
        //    {
        //        response.Body = "";
        //        response.Body += "<h3>You should first log in " + "- <a href='/Login'>Login</a></h3>";
        //    }
        //}
        //private static void LogoutAction(Request request, Response response)
        //{
        //    request.Session.Clear();
        //    response.Body = "";
        //    response.Body += "<h3>Logged out successfully!</h3>";
        //}
        
       

        
        //private static void AddFormDataAction
        //(Request request, Response response)
        //{
        //    response.Body = "";

        //    foreach (var (key, value) in request.Form)
        //    {
        //        response.Body += $"{key} - {value}";
        //        response.Body += Environment.NewLine;
        //    }
        //}

    }

    
}
