using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MassTransit;

using PoemMessage;

namespace PoemStats
{
    class StartMessageConsumer : IConsumer<PoemFilteringStarted>
    {
        public async Task Consume(ConsumeContext<PoemFilteringStarted> context)
        {
            Console.WriteLine("---------------------");
            int linesCount = context.Message.Poem.Split('\n').Length;
            Console.WriteLine("All lines: " + linesCount);
            Stats.GetInstance().SaveSourceLinesCount(linesCount, context.Message.CorrId);
            await Task.FromResult(1);
        }
    }
}
