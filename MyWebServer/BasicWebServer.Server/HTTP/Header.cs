namespace BasicWebServer.Server.HTTP
{
    public class Header
    {
        public Header(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; init; }
        public string Value { get; set; }
    }
}
