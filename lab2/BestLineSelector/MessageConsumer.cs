using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

using PoemFilterContract;
using PoemBeautifierContract;

namespace BestLineSelector
{
    public class MessageConsumer : IConsumer<ExtractBestLines>
    {
        public async Task Consume(ConsumeContext<ExtractBestLines> context)
        {
            string[] bestLines = BestLinesAnalyzer.ExtractBestLines(context.Message.Text, context.Message.VowelCounts, context.Message.ConsonantCount);
            
            Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], ConfigurationManager.AppSettings["PoemBeatifierManagerQueueName"]));
            var endpoint = await context.GetSendEndpoint(sendEndpointUri);
            await endpoint.Send<ProceedBeautifiedPoem>(new
            {
                UserId = context.Message.UserId,
                CorrId = context.Message.CorrId,
                Poem = bestLines
            });
        }
    }
}
