using System;
using Microsoft.Owin.Hosting;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using MassTransit;
using System.Runtime.Caching;

using PoemBeautifierContract;

namespace GoodPoem
{
    class Program
    {
        static void Main()
        {
            var storage = new Storage();
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
                        storage.Save(context.Message.UserId, context.Message.CorrId, string.Join("\n", context.Message.Poem));
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
