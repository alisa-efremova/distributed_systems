using System;
using System.Threading.Tasks;
using MassTransit;

using PoemMessage;

namespace BestLineSelector
{
   public class MessageConsumer : IConsumer<ConsonantsCalculated>
    {
        public async Task Consume(ConsumeContext<ConsonantsCalculated> context)
        {
            string line = Environment.NewLine + "Text " + context.Message.Text + ", count: " + context.Message.VowelCounts[0] + " " + context.Message.ConsonantCount[0];
            await Console.Out.WriteLineAsync(line);
        }
    }
}
