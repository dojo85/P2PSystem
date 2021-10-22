using System;
using System.Dynamic;

namespace FileExchangeSharedClasses
{
    public class FileEndPoint
    {
        public string IPAddress { get; set; }
        public int Port { get; set; }

        public FileEndPoint()
        {
            
        }

        public FileEndPoint(string ip, int port)
        {
            IPAddress = ip;
            Port = port;
        }
    }
}
