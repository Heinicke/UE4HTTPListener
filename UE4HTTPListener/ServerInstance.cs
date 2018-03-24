using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UE4HTTPListener
{
    public class ServerInstance
    {
        private string IP_ADDRESS;
        private string PORT;
        private string MATCHMAKING_ID;
        private string CURRENT_DIRECTORY;
        private string SERVER_EXE_NAME = "KreavianShooterServer.exe";
        private int PROCESS_ID_SERVER;
        private string gamemode = "DM";
        private string MaxPlayers = "6";
        private string RoundTime = "10";
        private int gracePeriod = 2;
        private string KillLimit = "15";
        private string FriendlyFire = "0";

        private static DateTime SERVER_START_TIMESTAMP;

        private string[] maps = new string[] { "/Game/GenericShooter/Maps/FiveRooms",
                                               "/Game/GenericShooter/Maps/ValAllar"};

        public ServerInstance(string address, string port, string matchMakingId, string currentDirectory)
        {
            IP_ADDRESS = address;
            PORT = port;
            MATCHMAKING_ID = matchMakingId;
            CURRENT_DIRECTORY = currentDirectory;
        }

        public bool StartServer(bool IsLiveServer)
        {
            bool started = false;
            //Console.WriteLine("--Starting Server--");
            //Process.Start(GeneratePath(), GenerateArguments()); 

            Process p = new Process();
            string path = GeneratePath();
            string args = GenerateArguments();
            p.StartInfo.FileName = path;
            p.StartInfo.Arguments = args;

            Console.WriteLine("--Starting Server--");
            Console.WriteLine(path);
            Console.WriteLine(args);

            started = p.Start();

            try
            {
                PROCESS_ID_SERVER = p.Id;

                SERVER_START_TIMESTAMP = DateTime.Now;
                MatchMakingMaster.registerPID(PROCESS_ID_SERVER, SERVER_START_TIMESTAMP.ToString("F"));
                Console.WriteLine("Server Started: {0} ID: {1}", started, PROCESS_ID_SERVER);

                //Only Start Death Timers for live servers, Not test ones
                if(IsLiveServer)
                {
                    Console.WriteLine("Server Death Timer Started: {0} minutes", StartDeathTimer().ToString());
                    MatchMakingMaster.totalServersCreated++;
                }
            }
            catch(InvalidOperationException)
            {
                started = false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                started = false;
            }

            return started;
        }

        public void StopServer()
        {
            Process pTOKill = Process.GetProcessById(PROCESS_ID_SERVER);
            if (pTOKill.ProcessName.Length != 0)
            {
                pTOKill.Kill();
                MatchMakingMaster.unregisterPID(PROCESS_ID_SERVER);
                MatchMakingMaster.removeMMServer(MATCHMAKING_ID);
                MatchMakingMaster.removeServer(PORT);
                Console.WriteLine("Server Instance '" + PROCESS_ID_SERVER + "' Terminated");
            }
        }

        public int GetProcessIdOfServer()
        {
            return PROCESS_ID_SERVER;
        }

        private string GenerateArguments()
        {
            //gamemode = "DM";
            //MaxPlayers = "6";
            //RoundTime = "10";
            //KillLimit = "15";
            //FriendlyFire = "0";
            string args;

            //Get Random Map
            Random random = new Random();
            int mapID = random.Next(0, maps.Length);

            args = maps[mapID]+"?game=" + gamemode + 
                               "?MaxPlayers="+ MaxPlayers + 
                               "?RoundTime=" + RoundTime +
                               "?KillLimit=" + KillLimit +
                               "?ff=" + FriendlyFire +
                               " -log -port=" + PORT;

            return args;
        }

        private string GeneratePath()
        {
            string path;

            path = string.Concat(CURRENT_DIRECTORY, "\\" , SERVER_EXE_NAME);

            return path;
        }

        private double StartDeathTimer()
        {
            //Get The Round Time Limit, Then add grace period minutes to allow
            //This allows for the round to end and all players to leave server
            //then the server will shut off.
            double deathtime = Double.Parse(RoundTime);
            deathtime += gracePeriod;

            Timer deathTimer = new Timer(TimeSpan.FromMinutes(deathtime).TotalMilliseconds);
            deathTimer.AutoReset = false;
            deathTimer.Elapsed += new ElapsedEventHandler(CallStopServer);
            deathTimer.Start();

            return deathtime;
        }

        private void CallStopServer(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Death Timer Elapsed. Stopping Server....");
            StopServer();
        }
    }
}
