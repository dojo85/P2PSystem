using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using FileExchangePeer.Client;
using FileExchangePeer.Server;

namespace FileExchangePeer
{
    class Program
    {
        private static string _path = @"D:\VSProjects\TECH\P2PExercise\P2PSystem\FileExchangePeer\MyFiles\";
        private static FileClient _client = new FileClient();

        private static bool _serverReady = false;

        static void Main(string[] args)
        {
            IPEndPoint serverEp = new IPEndPoint(IPAddress.Loopback, 10000);
            IPEndPoint clientEp = new IPEndPoint(IPAddress.Loopback, 11000);

            FileServer server = new FileServer(serverEp, _path);

            Task startServerTask = new Task(() => server.StartServer());
            startServerTask.Start();

            server.FilesRegistered += OnFilesRegistered;

            while (!_serverReady)
            {
                Thread.Sleep(500);
            }

            _client.Start();

            server.StopServer();

            Console.WriteLine("Press a key to close");
            Console.ReadKey();

        }

        private static void ReadUserInput(FileServer server)
        {
            string input = Console.ReadLine();

            if (input == "quit")
            {
                server.StopServer();
            }
            
        }

        private static void OnFilesRegistered(object o, EventArgs e)
        {
            _serverReady = true;
        }

    }
}
