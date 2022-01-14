using BasicWebServer.Server.HTTP;

namespace BasicWebServer.Server.Responses
{
    internal class NotFoundResponse : Response
    {
        public NotFoundResponse() : base(StatusCode.NotFound)
        {
        }
    }
}
