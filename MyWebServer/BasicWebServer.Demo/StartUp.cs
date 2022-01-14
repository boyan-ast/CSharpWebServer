using BasicWebServer.Server;
using BasicWebServer.Server.Responses;
using System;

namespace BasicWebServer.Demo
{
    internal class StartUp
    {
        static void Main(string[] args)
            => new HttpServer(routes => routes
                    .MapGet("/", new TextResponse("Hello from the server!"))
                    .MapGet("/HTML", new HtmlResponse("<h1>This is your HTML response</h1>"))
                    .MapGet("/Redirect", new RedirectResponse("https://softuni.org/")))
               .Start();
    }
}
