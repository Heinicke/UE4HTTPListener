using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace UE4HTTPListener
{
    class Program
    {
        private static readonly HttpListener listener = new HttpListener();
        private static readonly int port = 80;
        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static Dictionary<string, string> ServerList = new Dictionary<string, string>();
        [STAThread]
        static void Main(string[] args)
        {

            listener.Prefixes.Add("http://+:" + port +"/");
            listener.Start();
            Listen();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Server Public IP Address: {0}", GetPublicIPAddress());
            Console.WriteLine("Listening on port {0}", port);
            Console.WriteLine("Current Directory: {0}", currentDirectory);
            Console.WriteLine("Intilized. Press any key to exit...");
            Console.WriteLine("-----------------------------------");
            showAdminPanel();
            Console.ReadKey();

        }

        private static async void Listen()
        {
            bool listen = true;
            while (listen)
            {
                var context = await listener.GetContextAsync();
                DateTime timestamp = DateTime.Today;
                Console.WriteLine("Client connected at " + timestamp.ToString());
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
            string responseString = returnConnectionInfo();
            Console.WriteLine("Connection String Received - " + responseString);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            Console.WriteLine("Sending Response...");
            output.Write(buffer, 0, buffer.Length);
            Console.WriteLine("Response Sent!");
            output.Close();
        }

        private static string returnConnectionInfo()
        {
            string address = "127.0.0.1";
            string port = "7777";



            string connectionInfo = string.Concat(address,":", port);

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

        public static void registerServer(string address, string port)
        {
            ServerList.Add(port, address);
            //AdminPanel.refreshServerList();
            var adminPanelForm = Application.OpenForms.OfType<AdminPanel>().Single();
            adminPanelForm.refreshServerList();
        }

        public static void removeServer(string port)
        {
            ServerList.Remove(port);
            //AdminPanel.refreshServerList();
        }

        public static Dictionary<string, string> GetServerList()
        {
            return ServerList;
        }

        private static bool showAdminPanel()
        {
            //AdminPanel adminpanel = new AdminPanel();
            //adminpanel.Show();

            Application.EnableVisualStyles();
            Application.Run(new AdminPanel());

            return true;
        }
    }
}
