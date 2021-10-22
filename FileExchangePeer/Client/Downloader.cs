using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileExchangePeer.Client
{
    public class Downloader
    {
        private IPAddress _ip;
        private int _port;
        private string _fileName;

        private string _dirPath = @"D:\VSProjects\TECH\P2PExercise\P2PSystem\FileExchangePeer\MyIncomingFiles\";

        public Downloader(IPAddress ip, int port, string fileName)
        {
            _ip = ip;
            _port = port;
            _fileName = fileName;
        }

        public void GetFileFromServer()
        {
            TcpClient socket = new TcpClient("localhost", 11000);
            socket.Connect(_ip, _port);

            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);

            int thisRead = 0;
            int bytesPerRead = 1024;
            byte[] buffer = new byte[bytesPerRead];

            using (FileStream fs = File.Create(_dirPath + _fileName))
            {
                while (thisRead > 0)
                {
                    thisRead = ns.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, thisRead);
                }
                fs.Close();
                Console.WriteLine(">Filestream closed");
            }

            socket.Close();
            Console.WriteLine(">Connection closed");
            
        }
        

    }
}
