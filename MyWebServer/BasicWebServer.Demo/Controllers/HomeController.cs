using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using System.Text;
using System.Web;

namespace BasicWebServer.Demo.Controllers
{
    public class HomeController : Controller
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

        private const string DownloadForm = @"<form action='/Content' method='POST'>
   <input type='submit' value ='Download Sites Content' /> 
</form>";

        private const string FileName = "content.txt";

        public HomeController(Request request) : base(request)
        {
        }

        public Response Index() => this.Text("Hello from the server!");

        public Response Redirect() => this.Redirect("https://softuni.org/");

        public Response Html() => this.Html(HtmlForm);

        public Response HtmlFormPost()
        {
            string formData = string.Empty;

            foreach (var (key, value) in this.Request.Form)
            {
                formData += $"{key} - {value}";
                formData += Environment.NewLine;
            }

            return this.Text(formData);
        }

        public Response Content() => Html(DownloadForm);

        public Response DownloadContent()
        {
            DownloadSitesAsTextFile(FileName,
                new string[] { "https://judge.softuni.org/", "https://softuni.org"})
                .Wait();

            return this.File(FileName);
        }

        public Response Session()
        {
            string currentDateKey = "CurrentDate";
            bool sessionExists = this.Request.Session.ContainsKey(currentDateKey);

            if (sessionExists)
            {
                var currentDate = this.Request.Session[currentDateKey];

                return this.Text($"Stored date: {currentDate}!");
            }

            return Text("Current date stored!");
        }

        public Response Cookies()
        {
            if (this.Request.Cookies.Any(c => c.Name != Server.HTTP.Session.SessionCookieName))
            {
                var cookieText = new StringBuilder();

                cookieText.AppendLine("<h1>Cookies</h1>");

                cookieText.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in this.Request.Cookies)
                {
                    cookieText.Append("<tr>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookieText.Append("</tr>");
                }

                cookieText.Append("</table>");

                return this.Html(cookieText.ToString());
            }

            var cookies = new CookieCollection();
            cookies.Add("First-Cookie", "First-Cookie-Value");
            cookies.Add("Second-Cookie", "Second-Cookie-Value");

            return Html("<h1>Cookies set!</h1>", cookies);
        }

        private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            string[] responses = await Task.WhenAll(downloads);

            var responsesString = string.Join(Environment.NewLine + new string('-', 100), responses);

            await System.IO.File.WriteAllTextAsync(fileName, responsesString);
        }

        private static async Task<string> DownloadWebSiteContent(string url)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();

                return html.Substring(0, 2000);
            }
        }
    }
}
