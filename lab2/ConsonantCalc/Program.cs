using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

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

            Console.WriteLine("Consonant calc service is working... ");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            busControl.Stop();
        }
    }
}
