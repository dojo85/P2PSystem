using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using FileExchangeSharedClasses;

namespace FileExchangePeer.Client
{
    public class FileClient
    {
        private const string Divider = "================================================";

        private Downloader _downloader = null;
        private IPEndPoint _serverEp = new IPEndPoint(IPAddress.Loopback, 10000);

        private bool _isRunning = true;

        public FileClient()
        {
            
        }

        public void Start()
        {
            //Console.Write("Enter file name: ");
            //string fileName = Console.ReadLine();

            //_downloader = new Downloader(_serverEp, fileName);
            //_downloader.DownloadFile();
            //Console.WriteLine("Log: clientApp end...");

            UserInputLoop();
            
        }

        private void UserInputLoop()
        {
            while (_isRunning)
            {
                DisplayUserMenu();
                string input = Console.ReadLine();
                StartSelectedFunction(input);
            }

        }

        private void DisplayUserMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Select an option: ");
            Console.WriteLine("1. List available files");
            Console.WriteLine("2. Download a file");
            Console.WriteLine("3. Close application");
        }

        private void StartSelectedFunction(string input)
        {
            switch (input)
            {
                case "1":
                    var files = GetAvailableFiles();
                    PrintAllFilenames(files);
                    break;
                case "2":
                    DownloadFile();
                    break;
                case "3":
                    _isRunning = false;
                    break;
                default:
                    break;
            }
        }

        private void PrintAllFilenames(List<string> fileNames)
        {
            Console.Clear();
            Console.WriteLine(Divider);
            Console.WriteLine("\tAvailable files");
            Console.WriteLine(Divider);
            foreach (var file in fileNames)
            {
                Console.WriteLine("\t" + file);
            }
        }

        private List<string> GetAvailableFiles()
        {
            List<string> fileNames = new List<string>();
            using (HttpClient http = new HttpClient())
            {
                fileNames = http.GetFromJsonAsync<List<string>>("https://localhost:44378/files").Result;
            }
            return fileNames;
        }

        private void DownloadFile()
        {
            Console.Write("Enter the file name: ");
            string fileName = Console.ReadLine();

            try
            {
                FileEndPoint ep = GetFileEndPoint(fileName).Result;
                _downloader = new Downloader(new IPEndPoint(IPAddress.Parse(ep.IPAddress), ep.Port), fileName);
                _downloader.DownloadFile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("An error occurred. Try again.");
            }
        }

        private async Task<FileEndPoint> GetFileEndPoint(string fileName)
        {
            FileEndPoint ep = new FileEndPoint();
            using (HttpClient http = new HttpClient())
            {
                var result = await http.GetFromJsonAsync<List<FileEndPoint>>("https://localhost:44378/files/" + fileName);
                ep = result?[0];
            }
            return ep;
        }
    }
}
