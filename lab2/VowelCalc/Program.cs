using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Net.Http;
using MassTransit;
using System.Threading.Tasks;

namespace VowelCalc
{
    class Program
    {
        static void Main()
        {
            using (WebApp.Start<Startup>(url: ConfigurationManager.AppSettings["BaseAddress"]))
            {
                Console.WriteLine("Vowel calc service is working... ");
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
