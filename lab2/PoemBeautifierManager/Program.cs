using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Net.Http;
using MassTransit;

using PoemBeautifierContract;

namespace PoemBeautifierManager
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

                cfg.ReceiveEndpoint(host, ConfigurationManager.AppSettings["QueueName"], e =>
                {
                    e.Consumer<RejectPoemMessageConsumer>();
                });

            });
            busControl.Start();

            using (WebApp.Start<Startup>(url: ConfigurationManager.AppSettings["BaseAddress"]))
            {
                Console.WriteLine("Poem beautifier manager service is working... ");
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
            busControl.Stop();
        }
    }
}
