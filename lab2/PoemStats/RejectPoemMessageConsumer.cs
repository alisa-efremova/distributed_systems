using System;
using System.Threading.Tasks;
using MassTransit;
using System.Configuration;

using PoemBeautifierContract;

namespace PoemStats
{
    public class RejectPoemMessageConsumer : IConsumer<RejectPoem>
    {
        public async Task Consume(ConsumeContext<RejectPoem> context)
        {
            Console.WriteLine("Poem rejected");
            // TODO: remove poem
            await Task.FromResult(1);
        }
    }
}
