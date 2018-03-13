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
        static void Main(string[] args)
        {

            listener.Prefixes.Add("http://+:80/");
            listener.Start();
            Listen();
            Console.WriteLine("Listening...");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

        }

        private static async void Listen()
        {
            bool listen = true;
            while (listen)
            {
                var context = await listener.GetContextAsync();
                Console.WriteLine("Client connected");
                await Task.Factory.StartNew(() => ProcessRequest(context));
            }

            listener.Close();

        }

        private static void ProcessRequest(HttpListenerContext context)
        {
            System.Threading.Thread.Sleep(10 * 1000);
            Console.WriteLine("Generating Response");
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string responseString = "<html><body>" + returnConnectionInfo() + "</body></html>";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
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
