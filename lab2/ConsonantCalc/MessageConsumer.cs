using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

using Message;

namespace ConsonantCalc
{
    public class MessageConsumer : IConsumer<VowelsCalculated>
    {
        public async Task Consume(ConsumeContext<VowelsCalculated> context)
        {
            string line = Environment.NewLine + "Text " + context.Message.Text + ", count: " + context.Message.VowelCounts[0];
            await Console.Out.WriteLineAsync(line);
        }
    }
}
