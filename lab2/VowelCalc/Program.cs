using System;
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

            Console.WriteLine("Vowel calc selector service is working... ");
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            busControl.Stop();
        }
    }
}
