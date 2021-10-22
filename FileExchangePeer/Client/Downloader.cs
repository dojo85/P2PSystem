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
        private string _dirPath = @"D:\VSProjects\TECH\P2PExercise\P2PSystem\FileExchangePeer\MyIncomingFiles\";
        private string _fileName;
        private IPEndPoint _serverEndPoint;
        private IPEndPoint _localEndPoint = new IPEndPoint(IPAddress.Loopback, 11000);

        public Downloader(IPEndPoint serverEndPoint, string fileName)
        {
            _serverEndPoint = serverEndPoint;
            _fileName = fileName;
        }

        public void DownloadFile()
        {
            TcpClient client = new TcpClient(_localEndPoint);

            try
            {
                client.Connect(_serverEndPoint.Address, _serverEndPoint.Port);
                Console.WriteLine($"> Connected to: {_serverEndPoint.Address}:{_serverEndPoint.Port}");

                NetworkStream ns = client.GetStream();
                StreamReader reader = new StreamReader(ns);
                StreamWriter writer = new StreamWriter(ns);

                writer.WriteLine("get " + _fileName);
                writer.Flush();

                int thisRead = 1;
                int bytesPerRead = 1024;
                byte[] buffer = new byte[bytesPerRead];
                string fullPath = _dirPath + _fileName;
                //new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write)
                using (FileStream fs = File.Create(fullPath) )
                {
                    Console.WriteLine($"> Start download of: {_fileName}");
                    while (thisRead > 0)
                    {
                        thisRead = ns.Read(buffer, 0, buffer.Length);
                        fs.Write(buffer, 0, thisRead);
                    }
                    fs.Close();
                    Console.WriteLine("> Filestream closed");
                }
                client.Close();
                Console.WriteLine("> Disconnected");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

    }
}
