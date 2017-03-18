using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MassTransit;

using EventContract;

namespace PoemStats
{
    class StartMessageConsumer : IConsumer<PoemFilteringStarted>
    {
        public async Task Consume(ConsumeContext<PoemFilteringStarted> context)
        {
            int linesCount = context.Message.Poem.Length;
            Stats.GetInstance().SaveSourceLinesCount(linesCount, context.Message.CorrId);

            Console.WriteLine("---------------------");
            Console.WriteLine("All lines: " + linesCount);
            await Task.FromResult(1);
        }
    }
}
