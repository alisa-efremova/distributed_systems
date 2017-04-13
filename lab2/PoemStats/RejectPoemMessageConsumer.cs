using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

using PoemBeautifierContract;

namespace PoemStats
{
    public class RejectPoemMessageConsumer : IConsumer<RejectPoem>
    {
        private readonly Stats _stats;

        public RejectPoemMessageConsumer()
        {
            _stats = new Stats();
        }

        public async Task Consume(ConsumeContext<RejectPoem> context)
        {
            Console.WriteLine("Poem rejected");
            _stats.RemoveStats(context.Message.CorrId);
            await Task.FromResult(1);
        }
    }
}
