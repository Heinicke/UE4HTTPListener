using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UE4HTTPListener
{
    class Program
    {
        private static readonly HttpListener listener = new HttpListener();
        private static readonly int port = 80;
        static void Main(string[] args)
        {

            listener.Prefixes.Add("http://+:" + port +"/");
            listener.Start();
            Listen();
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Listening on port " + port + "...");
            Console.WriteLine("Press any key to exit...");
            Console.WriteLine("-----------------------------------");
            Console.ReadKey();

        }

        private static async void Listen()
        {
            bool listen = true;
            while (listen)
            {
                var context = await listener.GetContextAsync();
                Console.WriteLine("Client connected");
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
            string responseString = "<html><body>" + returnConnectionInfo() + "</body></html>";
            Console.WriteLine("Connection String Received");
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
    }
}
