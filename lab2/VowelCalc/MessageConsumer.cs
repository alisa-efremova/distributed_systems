using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

using PoemFilterContract;
using PoemBeautifierContract;

namespace VowelCalc
{
    public class MessageConsumer : IConsumer<ProceedOriginalPoem>
    {
        public async Task Consume(ConsumeContext<ProceedOriginalPoem> context)
        {
            Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], ConfigurationManager.AppSettings["ConsonantCalcQueueName"]));
            var endpoint = await context.GetSendEndpoint(sendEndpointUri);
            await endpoint.Send<CalculateConsonants>(new
            {
                UserId = context.Message.UserId,
                CorrId = context.Message.CorrId,
                Text = context.Message.Poem,
                VowelCounts = VowelCalculator.GetVowelCountPerLine(context.Message.Poem)
            });
        }
    }
}
