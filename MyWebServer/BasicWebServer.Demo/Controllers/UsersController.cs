using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;

namespace BasicWebServer.Demo.Controllers
{
    public class UsersController : Controller
    {
        private const string Username = "user";

        private const string Password = "1234";

        public UsersController(Request request) : base(request)
        {

        }
        public Response Login() => this.View();

        public Response LogInUser()
        {
            this.Request.Session.Clear();

            var usernameMatches = this.Request.Form["Username"] == Username;
            var passwordMatches = this.Request.Form["Password"] == Password;

            if (usernameMatches && passwordMatches)
            {
                if (!this.Request.Session.ContainsKey(Session.SessionUserKey))
                {
                    this.Request.Session[Session.SessionUserKey] = "MyUserId";

                    var cookies = new CookieCollection();
                    cookies.Add(Session.SessionCookieName, this.Request.Session.Id);

                    return this.Html("<h3>Logged successfully!</h3>", cookies);
                }

                return this.Html("<h3> Logged successfully!</h3>");
            }

            return this.Redirect("/Login");
        }

        public Response Logout()
        {
            this.Request.Session.Clear();

            return this.Html("<h3>Logged out successfully!</h3>");
        }

        public Response GetUserData()
        {
            if (this.Request.Session.ContainsKey(Session.SessionUserKey))
            {
                return this.Html($"<h3>Currently logged-in user is with username '{Username}'</h3>");
            }

            return this.Redirect("/Login");
        }
    }
}
