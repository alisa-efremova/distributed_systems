using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Net.Http;
using MassTransit;
using System.Threading.Tasks;

using PoemMessage;

namespace VowelCalc
{
    class Program
    {
        static void Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(ConfigurationManager.AppSettings["RabbitMQHost"]), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });
            });
            busControl.Start();

            using (WebApp.Start<Startup>(url: ConfigurationManager.AppSettings["BaseAddress"]))
            {
                Console.WriteLine("Vowel calc service is working... ");
                Console.WriteLine("Press Enter to exit.");

                while (true)
                {
                    string input = Console.ReadLine();
                    busControl.Publish<VowelsCalculated>(new 
                    { 
                        CorrId = "id",
                        Text = input,
                        VowelCounts = VowelCalculator.GetVowelCountPerLine(input)
                    });
                }
                
                busControl.Stop();
            }
        }
    }
}
