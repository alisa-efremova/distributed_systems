using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MassTransit;

using PoemBeautifierContract;

namespace PoemStats
{
    class CompleteMessageConsumer : IConsumer<ProceedBeautifiedPoem>
    {
        private readonly Stats _stats;

        public CompleteMessageConsumer()
        {
            _stats = new Stats();
        }

        public async Task Consume(ConsumeContext<ProceedBeautifiedPoem> context)
        {
            int linesCount = context.Message.Poem.Length;
            _stats.SaveGoodLinesCount(linesCount, context.Message.CorrId);

            Console.WriteLine("Good lines: " + linesCount);
            Console.WriteLine("Stats: " + _stats.GetGoodLinesPercent().ToString());
            await Task.FromResult(1);
        }
    }
}
