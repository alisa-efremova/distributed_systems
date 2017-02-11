using System;
using Microsoft.Owin.Hosting;
using System.Net.Http;

namespace BackendOwin
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Service is working... ");
                Console.WriteLine("Press Enter to exit.");

                Console.ReadLine(); 
            }
        }
    }
}
