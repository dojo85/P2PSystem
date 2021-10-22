using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileExchangePeer.Client
{
    public class FileClient
    {
        private Downloader _downloader = null;

        public FileClient()
        {

        }

        public void StartClientApp()
        {
            Console.Write("Enter file name: ");
            string fileName = Console.ReadLine();

            _downloader = new Downloader(IPAddress.Parse("127.0.0.1"), 10001, fileName);
            _downloader.GetFileFromServer();
            Console.WriteLine("Log: clientApp end...");
        }
    }
}
