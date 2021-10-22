using System;
using System.Net;
using System.Threading.Tasks;
using FileExchangePeer.Client;
using FileExchangePeer.Server;

namespace FileExchangePeer
{
    class Program
    {
        private static string _path = @"D:\VSProjects\TECH\P2PExercise\P2PSystem\FileExchangePeer\MyFiles\";
        static void Main(string[] args)
        {
            IPEndPoint serverEp = new IPEndPoint(IPAddress.Loopback, 10000);
            IPEndPoint clientEp = new IPEndPoint(IPAddress.Loopback, 11000);

            FileServer server = new FileServer(serverEp, _path);
            server.StartServer();

            //server.FilesRegistered += OnFilesRegistered;

            ReadUserInput(server);
            Console.WriteLine("You can shutdown now");
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
            FileClient clientApp = new FileClient();
            clientApp.StartClientApp();
        }
    }
}
