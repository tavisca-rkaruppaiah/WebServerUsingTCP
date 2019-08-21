using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace WebServer2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer _httpServer= new HttpServer(8080);
            _httpServer.StartServer();
        }
    }
}
