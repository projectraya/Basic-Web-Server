﻿using BasicWebServer.Demo.Models;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BasicWebServer.Demo.Controllers
{
    public class HomeController : Controller
    {
        
        private const string FileName = "context.txt";

        public HomeController(Request request) : base(request)
        {
        }

        //add actions (methods)
        public Response Index() => Text("Hello from the server!");

        public Response Redirect() => Redirect("https://softuni.org/");
        public Response Html() => View();

        public Response HtmlFormPost()
        {
            var name = this.Request.Form["Name"];
            var age = this.Request.Form["Age"];

            var model = new FormViewModel()
            {
                Name = name,
                Age = int.Parse(age)
            };
            return View(model);
        }

        public Response Content() => View();

        public Response DownloadContent()
        {
            DownloadSitesAsTextFilesAsync(FileName, 
                new string[] { "https://judge.softuni.org", "https://softuni.org" }).Wait();

            return File(FileName);
        }

        public Response Cookies()
        {

            if (this.Request.Cookies.Any(c => c.Name != BasicWebServer.Server.HTTP.Session.SessionCookieName))
            {
                StringBuilder cookieText = new StringBuilder();

                cookieText.AppendLine("<h1>Cookies</h1>");

                cookieText.AppendLine(@"<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in this.Request.Cookies)
                {
                    // Ignore ASP.NET Core system cookies
                    if (cookie.Name.StartsWith(".AspNetCore.") || cookie.Name.StartsWith("ASP.NET_SessionId"))
                    {
                        continue;
                    }

                    cookieText.Append("<tr>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("</tr>");
                }

                cookieText.Append("</table>");

                return Html(cookieText.ToString());
            }

            var cookies = new CookieCollection();

            cookies.Add("My-Cookie", "My-Value");
            cookies.Add("My-Second-Cookie", "My-Second-Value");

            return Html("<h1>No cookies yet!</h1>", cookies);
        }

        public Response Session()
        {
            string currentDateKey = "CurrentDate";

            bool sessionExists = this.Request.Session
                .ContainsKey(currentDateKey);

            if (sessionExists)
            {
                var currentDate = this.Request.Session[currentDateKey];
                return Text($"Stored date: {currentDate}!");
            }

            return Text("Current date stored!");
        }

        #region helper methods
        private static async Task DownloadSitesAsTextFilesAsync(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebsiteContentAsync(url));
            }

            var responses = await Task.WhenAll(downloads);
            var responsesString = string.Join
                (Environment.NewLine + new string('-', 100), responses);

            await System.IO.File.WriteAllTextAsync(fileName, responsesString);
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
        #endregion
    }
}
