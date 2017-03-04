using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Owin.Hosting;
using System.Configuration;

using Message;

namespace ConsonantCalc
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
                    e.Consumer<MessageConsumer>();
                });
            });
            busControl.Start();

            using (WebApp.Start<Startup>(url: ConfigurationManager.AppSettings["BaseAddress"]))
            {
                Console.WriteLine("Consonant calc service is working... ");
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();

                busControl.Stop();
            }

        }
    }
}
