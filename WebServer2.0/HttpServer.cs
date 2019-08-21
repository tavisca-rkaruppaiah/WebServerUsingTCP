using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WebServer2._0
{
    public class HttpServer
    {
        public const string MSG_DIR = @"\root\msg\";
        public const string WEB_DIR = @"\root\web\";
        public const string VERSION = "HTTP/1.1";
        public const string NAME = "R HttpServer2.0";
        private bool _isRunning = false;
        private TcpListener _listener;


        public HttpServer(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void StartServer()
        {
            Thread _serverThread = new Thread(new ThreadStart(Run));
            _serverThread.Start();
        }

        public void Run()
        {
            _isRunning = true;
            _listener.Start();

            while(_isRunning)
            {
                Console.WriteLine("Waiting for connection....");
                TcpClient _client = _listener.AcceptTcpClient();
                Console.WriteLine("Client Connected" + _client);

                ManageClient(_client);

                _client.Close();


            }

            _isRunning = false;

            _listener.Stop();

        }

        private void ManageClient(TcpClient client)
        {
            StreamReader _reader = new StreamReader(client.GetStream());
            string _message = "";
            while(_reader.Peek() != -1)
            {
                _message += _reader.ReadLine() + "\n";
            }

            //Console.WriteLine("HttpRequest "+ _message);
            HttpRequest _httpRequest = HttpRequest.GetHttpRequest(_message);
            HttpResponse _httpResponse = HttpResponse.ProcessRequest(_httpRequest);
            _httpResponse.ViewResponse(client.GetStream());
        }
    }
}
