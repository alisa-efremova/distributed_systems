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
    class StartMessageConsumer : IConsumer<ProceedOriginalPoem>
    {
        private readonly Stats _stats;

        public StartMessageConsumer()
        {
            _stats = new Stats();
        }

        public async Task Consume(ConsumeContext<ProceedOriginalPoem> context)
        {
            int linesCount = context.Message.Poem.Length;
            _stats.SaveSourceLinesCount(linesCount, context.Message.CorrId);

            Console.WriteLine("---------------------");
            Console.WriteLine("All lines: " + linesCount);
            await Task.FromResult(1);
        }
    }
}
