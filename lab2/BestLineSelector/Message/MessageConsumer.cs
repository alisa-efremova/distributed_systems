using System;
using System.Threading.Tasks;
using MassTransit;

using PoemMessage;

namespace BestLineSelector
{
    public class MessageConsumer : IConsumer<ExtractBestLines>
    {
        private readonly RedisStorage _storage;

        public MessageConsumer()
        {
            _storage = new RedisStorage();
        }

        public async Task Consume(ConsumeContext<ExtractBestLines> context)
        {
            string bestLines = BestLinesAnalyzer.ExtractBestLines(context.Message.Text, context.Message.VowelCounts, context.Message.ConsonantCount);
            await context.Publish<PoemFilteringCompleted>(new
            {
                CorrId = context.Message.CorrId,
                Poem = bestLines
            });
            _storage.Save(context.Message.CorrId, bestLines);
            await Task.FromResult(1);
        }
    }
}
