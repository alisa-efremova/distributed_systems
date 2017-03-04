using System;
using System.Threading.Tasks;
using MassTransit;

using PoemMessage;

namespace BestLineSelector
{
    public class MessageConsumer : IConsumer<ConsonantsCalculated>
    {
        private readonly RedisStorage _storage;

        public MessageConsumer()
        {
            _storage = new RedisStorage();
        }

        public async Task Consume(ConsumeContext<ConsonantsCalculated> context)
        {
            string bestLines = BestLinesAnalyzer.ExtractBestLines(context.Message.Text, context.Message.VowelCounts, context.Message.ConsonantCount);
            _storage.Save(context.Message.CorrId, bestLines);
            await Task.FromResult(1);
        }
    }
}
