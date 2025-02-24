using BasicWebServer.Server;
using BasicWebServer.Server.HTTP;
using BasicWebServer.Server.Responses;
using BasicWebServer.Server.Routing;
using System.Runtime.CompilerServices;

namespace BasicWebServer.Demo
{
    public class StartUp
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

        private const string DownloadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
</form>";

        private const string FileName = "context.txt";

        public async static Task Main()
        {
            await DownloadSitesAsTextFilesAsync(StartUp.FileName,
                ["https://judge.softuni.org/", "https://softuni.org/"]);

            HttpServer server = new HttpServer(routes => routes
            .MapGet("/", new TextResponse("Hello from the server!"))
            .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
            .MapGet("/HTML", new HTMLResponse(HtmlForm))
            .MapPost("/HTML", new TextResponse("", StartUp.AddFormDataAction))
            .MapGet("/Content", new HTMLResponse(StartUp.DownloadForm))
            .MapPost("/Content", new TextFileResponse(StartUp.FileName)));

            await server.StartAsync();
        }
        
        private static async Task<string> DownloadWebsiteContentAsync(string url)
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                return html.Substring(0, 2000);
            }
        }

        private static async Task DownloadSitesAsTextFilesAsync(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach(var url in urls)
            {
                downloads.Add(DownloadWebsiteContentAsync(url));
            }

            string[] responses = await Task.WhenAll(downloads);
            string responsesString = string.Join
                (Environment.NewLine + new string('-', 100), responses);

            await File.WriteAllTextAsync(fileName, responsesString);
        }
        private static void AddFormDataAction
        (Request request, Response response)
        {
            response.Body = "";

            foreach (var (key, value) in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;
            }
        }
    }

    
}
