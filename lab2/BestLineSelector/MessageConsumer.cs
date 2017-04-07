using System;
using System.Threading.Tasks;
using MassTransit;

using PoemFilterContract;
using PoemBeautifierContract;

namespace BestLineSelector
{
    public class MessageConsumer : IConsumer<ExtractBestLines>
    {
        public async Task Consume(ConsumeContext<ExtractBestLines> context)
        {
            string[] bestLines = BestLinesAnalyzer.ExtractBestLines(context.Message.Text, context.Message.VowelCounts, context.Message.ConsonantCount);
            await context.Publish<PoemFilteringCompleted>(new
            {
                UserId = context.Message.UserId,
                CorrId = context.Message.CorrId,
                Poem = bestLines
            });
        }
    }
}
