using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FileExchangePeer.Client;
using FileExchangeSharedClasses;

namespace FileExchangePeer.Server
{
    public class FileServer
    {
        private string _path;
        private List<string> _files;
        private FileEndPoint _endPoint;
        private ClientHandler _clientHandler;

        public event EventHandler FilesRegistered;

        public FileServer(IPEndPoint serverEndPoint, string path)
        {
            _endPoint = new FileEndPoint(serverEndPoint.Address.ToString(), serverEndPoint.Port);
            _path = path;
            _clientHandler = new ClientHandler(serverEndPoint.Address, serverEndPoint.Port, path);
        }
       
        public void StartServer()
        {
            _files = GetFileNames();
            Task register = new Task(() => RegisterFiles());
            register.Start();
            register.Wait();
            FilesRegistered?.Invoke(this, EventArgs.Empty);
            _clientHandler.StartListen();
            
        }

        public void StopServer()
        {
            _clientHandler.StopListen();
            Task unregister = new Task(() => UnregisterFiles());

            unregister.Start();
            unregister.Wait();
            Console.WriteLine("Files unregistered");
        }

        private async Task RegisterFiles()
        {
            using (HttpClient http = new HttpClient())
            {
                foreach (var file in _files)
                {
                    var response = await http.PostAsJsonAsync($"https://localhost:44378/files/{file}", _endPoint);
                }
            }
        }

        private async Task<bool> UnregisterFiles()
        {
            using (HttpClient http = new HttpClient())
            {
                foreach (var file in _files)
                {
                    var response = await http.PutAsJsonAsync($"https://localhost:44378/files/{file}", _endPoint);
                }
            }
            
            return true;
        }

        private List<string> GetFileNames()
        {
            var files = Directory.GetFiles(_path);
            List<string> fileNames = new List<string>();
            foreach (var file in files)
            {
                var split = file.Split('\\');
                string fileName = split[^1];
                //Console.WriteLine(fileName);
                fileNames.Add(fileName);
            }
            return fileNames;
        }
    }
}
