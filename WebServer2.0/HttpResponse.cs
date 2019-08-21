using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace WebServer2._0
{
    public class HttpResponse
    {
        private byte[] _data = null;
        private string _status;
        private string _mime;
        private HttpResponse(string status, string mime, byte[] data)
        {
            _data = data;
            _status = status;
            _mime = mime;

            //Console.WriteLine("status" + _status);
            
        }

        public static HttpResponse ProcessRequest(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                return ThrowNullMessage();

            if (httpRequest._type == "GET")
            {
                string _file = Environment.CurrentDirectory + HttpServer.WEB_DIR;
                //Console.WriteLine("file path"+_file);

                FileInfo _fileInfo = new FileInfo(_file);
                if(_fileInfo.Exists && _fileInfo.Extension.Contains('.'))
                {
                    MakeAFile(_fileInfo);
                }
                else
                {
                    DirectoryInfo _directoryInfo = new DirectoryInfo(_fileInfo + @"\");
                    if (!_directoryInfo.Exists)
                        return PageNotFound();
                    FileInfo[] _fileInfos = _directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in _fileInfos)
                    {
                        //Console.WriteLine("file : " + fileInfo);
                        string _fileName = fileInfo.Name;
                        //Console.WriteLine("filename : " + _fileName);
                        if (_fileName.Contains("default.htm") || _fileName.Contains("default.html") || _fileName.Contains("index.htm") || _fileName.Contains("index.html"))
                        {
                            //Console.WriteLine("ulla " + fileInfo);
                            return MakeAFile(fileInfo);
                        }
                    }
                }

            }
            else
                return NotAlowed();
            return PageNotFound();
        }

        private static HttpResponse MakeAFile(FileInfo fileInfo)
        {
        
            FileStream _fileStream = fileInfo.OpenRead();
            BinaryReader _binaryReader = new BinaryReader(_fileStream);
            Console.WriteLine("Make a file" + fileInfo);

            byte[] b = new byte[_fileStream.Length];
            _binaryReader.Read(b, 0, b.Length);
            _fileStream.Close();
            return new HttpResponse("200 OK", "text/html", b);
        }

        private static HttpResponse ThrowNullMessage()
        {
            string _file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "400.html";
            FileInfo _fileInfo = new FileInfo(_file);
            FileStream _fileStream = _fileInfo.OpenRead();
            BinaryReader _binaryReader = new BinaryReader(_fileStream);

            byte[] b = new byte[_fileStream.Length];
            _binaryReader.Read(b, 0, b.Length);
            _fileStream.Close();
            return new HttpResponse("400 Bad Request", "text/html", b);
        }

        private static HttpResponse NotAlowed()
        {
            string _file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "405.html";
            FileInfo _fileInfo = new FileInfo(_file);
            FileStream _fileStream = _fileInfo.OpenRead();
            BinaryReader _binaryReader = new BinaryReader(_fileStream);

            byte[] b = new byte[_fileStream.Length];
            _binaryReader.Read(b, 0, b.Length);
            _fileStream.Close();
            return new HttpResponse("400 Bad Request", "text/html", b);
        }
        private static HttpResponse PageNotFound()
        {
            string _file = Environment.CurrentDirectory + HttpServer.MSG_DIR + "404.html";
            FileInfo _fileInfo = new FileInfo(_file);
            FileStream _fileStream = _fileInfo.OpenRead();
            BinaryReader _binaryReader = new BinaryReader(_fileStream);

            byte[] b = new byte[_fileStream.Length];
            _binaryReader.Read(b, 0, b.Length);
            _fileStream.Close();
            return new HttpResponse("404 Page Not Found", "text/html", b);
        }

        public void ViewResponse(NetworkStream networkStream)
        {
            StreamWriter _streamWriter = new StreamWriter(networkStream);
            _streamWriter.WriteLine(string.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges: bytes\r\nContent-Length: {4}\r\n", HttpServer.VERSION, _status, HttpServer.NAME, _mime, _data.Length));
            _streamWriter.Flush();
            networkStream.Write(_data, 0, _data.Length);
           
        }
    }
}
