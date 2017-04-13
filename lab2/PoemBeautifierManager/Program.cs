using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Net.Http;

namespace PoemBeautifierManager
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start<Startup>(url: ConfigurationManager.AppSettings["BaseAddress"]))
            {
                Console.WriteLine("Poem beautifier manager service is working... ");
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
