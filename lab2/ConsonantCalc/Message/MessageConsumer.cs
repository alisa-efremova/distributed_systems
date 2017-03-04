using System.Threading.Tasks;
using MassTransit;

using PoemMessage;

namespace ConsonantCalc
{
    public class MessageConsumer : IConsumer<VowelsCalculated>
    {
        public async Task Consume(ConsumeContext<VowelsCalculated> context)
        {
            await context.Publish<ConsonantsCalculated>(new
            {
                CorrId = context.Message.CorrId,
                Text = context.Message.Text,
                VowelCounts = context.Message.VowelCounts,
                ConsonantCount = ConsonantCalculator.GetConsonantCountPerLine(context.Message.Text)
            });
        }
    }
}
