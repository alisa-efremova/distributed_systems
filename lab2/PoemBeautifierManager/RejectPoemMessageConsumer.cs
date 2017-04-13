using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

using PoemBeautifierContract;

namespace PoemBeautifierManager
{
    public class RejectPoemMessageConsumer : IConsumer<RejectPoem>
    {
        public async Task Consume(ConsumeContext<RejectPoem> context)
        {
            Console.WriteLine("Poem rejected");
            Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], ConfigurationManager.AppSettings["PoemStatsQueueName"]));
            var endpoint = await context.GetSendEndpoint(sendEndpointUri);
            await endpoint.Send<RejectPoem>(new
            {
                CorrId = context.Message.CorrId,
            });
        }
    }
}
