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
    class CompleteMessageConsumer : IConsumer<PoemFilteringCompleted>
    {
        public async Task Consume(ConsumeContext<PoemFilteringCompleted> context)
        {
            int linesCount = context.Message.Poem.Split('\n').Length - 1;
            Console.WriteLine("Good lines: " + linesCount);
            Stats.GetInstance().SaveGoodLinesCount(linesCount, context.Message.CorrId);
            Console.WriteLine("Stats: " + Stats.GetInstance().GetGoodLinesPercent().ToString());
            await Task.FromResult(1);
        }
    }
}
