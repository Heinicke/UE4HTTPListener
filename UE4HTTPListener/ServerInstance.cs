using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UE4HTTPListener
{
    public class ServerInstance
    {
        private static string IP_ADDRESS;
        private static string PORT;
        private static string MATCHMAKING_ID;
        private static string CURRENT_DIRECTORY;
        private static string SERVER_EXE_NAME = "KreavianShooter.exe";
        public ServerInstance(string address, string port, string matchMakingId, string currentDirectory)
        {
            IP_ADDRESS = address;
            PORT = port;
            MATCHMAKING_ID = matchMakingId;
            CURRENT_DIRECTORY = currentDirectory;
        }

        public static void StartServer()
        {
            Console.WriteLine("--Starting Server--");
            Process.Start(GeneratePath(), GenerateArguments());
        }

        private static string GenerateArguments()
        {
            string args;

            args = "www.northwindtraders.com";

            return args;
        }

        private static string GeneratePath()
        {
            string path;

            path = string.Concat(CURRENT_DIRECTORY, "\\" , SERVER_EXE_NAME);

            return path;
        }
    }
}
