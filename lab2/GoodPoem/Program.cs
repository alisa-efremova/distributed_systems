using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using MassTransit;

using PoemMessage;

namespace GoodPoem
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
                    e.Handler<PoemFilteringCompleted>(context =>
                    {
                        Console.WriteLine("Poem filtering completed");
                        return Task.FromResult(1);
                    });
                });

            });
            busControl.Start();

            using (WebApp.Start<Startup>(url: ConfigurationManager.AppSettings["BaseAddress"]))
            {
                Console.WriteLine("Good poem service is working... ");
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }

            busControl.Stop();
        }
    }
}
