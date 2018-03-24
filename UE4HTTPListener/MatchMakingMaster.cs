/****************************************************\
 * Author: Samuel Heinicke                          *
 * This code is the sole property of the Author(s)  *
 * Date: 3/17/2018                                  *
 * Match Making Server for use in multiplayer games *
\****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Diagnostics;

namespace UE4HTTPListener
{
    class MatchMakingMaster
    {
        private static readonly HttpListener listener = new HttpListener();
        private static readonly int port = 80;
        private static readonly int serverStartingPort = 7777;
        private static readonly int serverMaxPort = 7800;
        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string authenticationToken = "ZBz9IGM0KHm72BTmPslbXg0kpg4Rtr2U";

        public static int totalServersCreated = 0;

        //Header Value Names
        private static readonly string matchMakingIDString = "matchId";
        private static readonly string authTokenString = "authtoken";

        //Server List. Key: Port / Value: Address
        //MMList       Key: ID   / Value: Port
        private static Dictionary<string, string> ServerList = new Dictionary<string, string>();
        private static Dictionary<string, string> MMList = new Dictionary<string, string>();
        private static Dictionary<int, string> serverProcessList = new Dictionary<int, string>();
        [STAThread]
        static void Main(string[] args)
        {

            listener.Prefixes.Add("http://+:" + port +"/");
            listener.Start();
            Listen();
            Console.WriteLine("Copyright Kreavian - Code Author: Samuel Heinicke");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("| Server Public IP Address: {0}", GetPublicIPAddress());
            Console.WriteLine("| Listening on port {0}", port);
            Console.WriteLine("| Server Port Range {0} - {1}", serverStartingPort, serverMaxPort);
            Console.WriteLine("| Current Directory: {0}", currentDirectory);
            Console.WriteLine("| Authentication Token: {0}", authenticationToken);
            Console.WriteLine("| Headers: {0}, {1}", matchMakingIDString, authTokenString);
            Console.WriteLine("| Unreal Engine MatchMaking Server Initilized");
            Console.WriteLine("-------------------------------------------------");
            showAdminPanel();
            Console.ReadKey();

        }

        private static async void Listen()
        {
            bool listen = true;
            while (listen)
            {
                var context = await listener.GetContextAsync();
                DateTime timestamp = DateTime.Now;
                Console.WriteLine("Client connected at " + timestamp.ToString("F"));
                Console.WriteLine("Proccessing Request");
                await Task.Factory.StartNew(() => ProcessRequest(context));
            }

            listener.Close();

        }

        private static void ProcessRequest(HttpListenerContext context)
        {
            System.Threading.Thread.Sleep(10 * 1000);
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            NameValueCollection headers = request.Headers;

            if(!(headers.Get(authTokenString) == authenticationToken))
            {
                Console.WriteLine("Connection Refused {0} rejected", authTokenString);
                string responseString = "Connection Refused Authorization Token rejected";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                response.StatusCode = 403;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            else if(headers.Get(matchMakingIDString) == null)
            {
                Console.WriteLine("Connection Refused - Header {0} not set!", matchMakingIDString);
                string responseString = "Connection Refused MatchMaking ID not set.";
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                response.StatusCode = 400;
                output.Close();
            }
            else
            {
                string matchmakingID = headers.Get(matchMakingIDString);

                if(IsSameMMRequest(matchmakingID))
                {
                    Console.WriteLine("Same Matchmaking Request - Retrieving connection info");
                    string responseString = returnConnectionInfo(matchmakingID, false);
                    Console.WriteLine("Connection String Received - " + responseString);
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    Console.WriteLine("Sending Response...");
                    response.StatusCode = 200;
                    output.Write(buffer, 0, buffer.Length);
                    Console.WriteLine("Response Sent!");
                    output.Close();

                }
                else
                {
                    Console.WriteLine("Matchmaking Code - {0}", matchmakingID);
                    string responseString = returnConnectionInfo(matchmakingID, true);
                    Console.WriteLine("Connection String Received - " + responseString);
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    Console.WriteLine("Sending Response...");
                    response.StatusCode = 200;
                    output.Write(buffer, 0, buffer.Length);

                    Console.WriteLine("Response Sent!");
                    output.Close();
                }

            }
        }

        private static string returnConnectionInfo(string MatchMakingID, bool NewRequest)
        {
            string connectionInfo = "";
            if (NewRequest)
            {
                connectionInfo = InitiateServer(MatchMakingID);
            }
            else
            {
                string port;
                string address;
                MMList.TryGetValue(MatchMakingID, out port);
                ServerList.TryGetValue(port, out address);

                connectionInfo = string.Concat(address, ":", port);

            }
            
            return connectionInfo;
        }

        private static string GetPublicIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }

        private static bool IsSameMMRequest(string id)
        {

            return MMList.ContainsKey(id);
        }
        private static void registerServer(string address, string port)
        {
            if(!ServerList.ContainsKey(port))
            {
                ServerList.Add(port, address);
            }

            //var adminPanelForm = Application.OpenForms.OfType<AdminPanel>().Single();
            //adminPanelForm.refreshServerList();
        }

        private static void registerMMServer(string id, string port)
        {
            if (!MMList.ContainsKey(id))
            {
                MMList.Add(id, port);
            }

            //var adminPanelForm = Application.OpenForms.OfType<AdminPanel>().Single();
            //adminPanelForm.refreshServerList();
        }

        public static void registerPID(int pid, string date)
        {
            serverProcessList.Add(pid, date);
        }

        public static void unregisterPID(int pid)
        {
            serverProcessList.Remove(pid);
        }

        public static void removeServer(string port)
        {
            ServerList.Remove(port);
        }

        public static void removeMMServer(string matchid)
        {
            MMList.Remove(matchid);
        }

        public static string FindAvaiblePort()
        {
            int foundport = serverStartingPort;
            if (!ServerList.ContainsKey(serverStartingPort.ToString()))
            {
                foundport = serverStartingPort;
            }
            else
            {
                while(ServerList.ContainsKey(foundport.ToString()) && foundport <= serverMaxPort)
                {
                    foundport++;
                }
            }
            return foundport.ToString();
        }
        private static string InitiateServer(string matchMakingID)
        {
            //Port is avaible, registers the server.
            string foundPort = FindAvaiblePort();
            registerServer(GetPublicIPAddress(), foundPort);
            registerMMServer(matchMakingID, foundPort);
            string conn = string.Concat(GetPublicIPAddress(), ":", foundPort);
            //Start The Server Instance
            ServerInstance serverInstance = new ServerInstance(GetPublicIPAddress(), foundPort, matchMakingID, "D:\\Troy-Heinicke\\PackagedProjects\\KreavianShooter\\Windows\\WindowsNoEditor\\KreavianShooter\\Binaries\\Win64");
            serverInstance.StartServer(true);

            return conn;
        }
         
        public static void KillServerInstanceByID (int ID)
        {
            Process pTOKill = Process.GetProcessById(ID);
            if(pTOKill.ProcessName.Length != 0)
            {
                pTOKill.Kill();
                unregisterPID(ID);
                Console.WriteLine("Server Instance '" + ID + "' Terminated");
            }
        }

        public static Dictionary<string, string> GetServerList()
        {
            return ServerList;
        }
        public static Dictionary<string, string> GetMMServerList()
        {
            return MMList;
        }

        public static Dictionary<int, string> GetProcessList()
        {
            return serverProcessList;
        }

        private static bool showAdminPanel()
        {
            Application.EnableVisualStyles();
            Application.Run(new AdminPanel());

            return true;
        }
    }
}
