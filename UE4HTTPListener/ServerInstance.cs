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
        private static string SERVER_EXE_NAME = "KreavianShooterServer.exe";
        private static int PROCESS_ID_SERVER;
        private static DateTime SERVER_START_TIMESTAMP;

        public ServerInstance(string address, string port, string matchMakingId, string currentDirectory)
        {
            IP_ADDRESS = address;
            PORT = port;
            MATCHMAKING_ID = matchMakingId;
            CURRENT_DIRECTORY = currentDirectory;
        }

        public bool StartServer()
        {
            bool started = false;
            //Console.WriteLine("--Starting Server--");
            //Process.Start(GeneratePath(), GenerateArguments()); 

            Process p = new Process();
            p.StartInfo.FileName = GeneratePath();
            p.StartInfo.Arguments = GenerateArguments();

            started = p.Start();

            try
            {
                PROCESS_ID_SERVER = p.Id;

                SERVER_START_TIMESTAMP = DateTime.Now;
                MatchMakingMaster.registerPID(PROCESS_ID_SERVER, SERVER_START_TIMESTAMP.ToString("F"));
            }
            catch(InvalidOperationException)
            {
                started = false;
            }
            catch(Exception ex)
            {
                started = false;
            }

            return started;
        }

        public int GetProcessIdOfServer()
        {
            return PROCESS_ID_SERVER;
        }

        private static string GenerateArguments()
        {
            string args;

            args = "-log -port=" + PORT;

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
