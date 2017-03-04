using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Net.Http;

namespace VowelCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = ConfigurationManager.AppSettings["baseAddress"];

            // Start OWIN host 
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Vowel calc is working... ");
                Console.WriteLine("Press Enter to exit.");

                Console.ReadLine(); 
            }
        }
    }
}
