namespace WebServer2._0
{
    public class HttpRequest
    {
        public string _type { get; set; }
        public string _URL { get; set; }
        public string _host { get; set; }
        private HttpRequest(string type, string url, string host)
        {
             _type = type;
            _URL = url;
            _host = host;
            /*Console.WriteLine("type : "+type);
            Console.WriteLine("url : "+url);
            Console.WriteLine("host : "+host);*/
        }

        public static HttpRequest GetHttpRequest(string request)
        {
            if (string.IsNullOrEmpty(request))
                return null;
            string[] _tokens = request.Split(' ');
            string type = _tokens[0];
            

            string[] _u = _tokens[3].Split('C');
            string[] _h = _tokens[19].Split('8');
            string Url = _u[0];
            string host = _h[0]+'8';


            return new HttpRequest(type, Url, host);
        }
    }
}
