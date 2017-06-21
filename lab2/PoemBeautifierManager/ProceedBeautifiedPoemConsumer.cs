using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

using PoemBeautifierContract;

namespace PoemBeautifierManager
{
    class ProceedBeautifiedPoemConsumer : IConsumer<ProceedBeautifiedPoem>
    {
        public async Task Consume(ConsumeContext<ProceedBeautifiedPoem> context)
        {
            await SendMessage(ConfigurationManager.AppSettings["GoodPoemQueueName"], context);
            await SendMessage(ConfigurationManager.AppSettings["PoemStatsQueueName"], context);
        }

        private async Task SendMessage(string queue, ConsumeContext<ProceedBeautifiedPoem> context)
        {
            Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], queue));
            var endpoint = await context.GetSendEndpoint(sendEndpointUri);
            await endpoint.Send<ProceedBeautifiedPoem>(new
            {
                UserId = context.Message.UserId,
                CorrId = context.Message.CorrId,
                Poem = context.Message.Poem
            });
        }
    }
}
