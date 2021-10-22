using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileExchangePeer.Server
{
    public class ClientHandler
    {
        private IPAddress _ip;
        private int _port;
        private TcpListener _listener;
        private string _path;

        public ClientHandler(IPAddress ip, int port, string path)
        {
            _ip = ip;
            _port = port;
            _listener = new TcpListener(ip, port);
            _path = path;
        }

        public void StartListen()
        {
            _listener.Start();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Server ready");
            Console.ResetColor();
            Console.WriteLine($"Server address: {_ip}:{_port}");

            while (true)
            {
                TcpClient socket = _listener.AcceptTcpClient();
                Task.Run(() => HandleClient(socket));
            }
        }

        private void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            try
            {
                string requestLine = reader.ReadLine();
                string fileName = GetFileNameFromRequest(requestLine);
                string filePath = GetFilePath(fileName);
                socket.Client.SendFile(filePath);
            }
            catch (ArgumentException e)
            {
                writer.WriteLine(e.Message);
                writer.Flush();
                
            }
            catch (Exception e)
            {
                writer.WriteLine($"An error occurred. {e.Message}");
                writer.Flush();
            }
            finally
            {
                socket.Close();
            }

            
        }


        /// <summary>
        /// Checks if the request is a valid get request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A the file name of the requested file or null if request is not valid.</returns>
        private string GetFileNameFromRequest(string request)
        {
            var split = request.Split(" ");
            if (split[0].ToLower() == "get")
            {
                return split[1];
            }
            throw new ArgumentException($"Unknown command: {split[0]}");
        }

        /// <summary>
        /// Finds the path to the requested file.
        /// </summary>
        /// <param name="fileName"></param>
        private string GetFilePath(string fileName)
        {
            var fileDirectory = Directory.GetFiles(_path);
            var filePath = fileDirectory.FirstOrDefault(f => f.EndsWith(fileName));
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentException($"File not found: {fileName}");
            return filePath;
        }

        public void StopListen()
        {
            _listener.Stop();
        }

        
    }
}
