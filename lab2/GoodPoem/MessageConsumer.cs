using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

using PoemBeautifierContract;

namespace GoodPoem
{
    public class MessageConsumer : IConsumer<ProceedBeautifiedPoem>
    {
        private readonly Storage _storage;

        public MessageConsumer()
        {
            _storage = new Storage();
        }

        public async Task Consume(ConsumeContext<ProceedBeautifiedPoem> context)
        {
            Console.WriteLine("Poem filtering completed");
            bool isPoemSaved = false;

            try
            {
                _storage.Save(context.Message.UserId, context.Message.CorrId, string.Join("\n", context.Message.Poem));
                isPoemSaved = true;
            }
            catch (Exception)
            {
            }

            if (!isPoemSaved)
            {
                Console.WriteLine("Failed to save poem to database");

                Uri sendEndpointUri = new Uri(string.Concat(ConfigurationManager.AppSettings["RabbitMQHost"], ConfigurationManager.AppSettings["PoemBeatifierManagerQueueName"]));
                var endpoint = await context.GetSendEndpoint(sendEndpointUri);
                await endpoint.Send<RejectPoem>(new
                {
                    CorrId = context.Message.CorrId,
                });
            }
            else
            {
                await Task.FromResult(1);
            }
        }
    }
}
