using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Client
{
    internal class Program
    {
        private static void Main()
        {
            var listener = new HttpListener();
            const string host = "http://192.168.1.42:9001/";
            listener.Prefixes.Add(host);
            listener.Start();
            Console.WriteLine("Listening on {0}...", host);

            new Thread(() =>
                {
                    Console.ReadLine();
                    Environment.Exit(0);
                }).Start();

            while (true)
            {
                var context = listener.GetContext();
                var response = context.Response;
                var request = context.Request;
                var query = request.QueryString["q"];
                Console.WriteLine("{0} - {1}", DateTime.Now, query);
                var res = "";

                if (query != null)
                {
                    if (Regex.IsMatch(query, "Share")) res = "Point";
                }

                var buffer = Encoding.UTF8.GetBytes(res);
                response.ContentLength64 = buffer.Length;
                var output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
        }
    }
}

