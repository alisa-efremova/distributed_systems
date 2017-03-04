using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message
{
    public interface VowelsCalculated
    {
        string CorrId { get; }
        string Text { get; }
        int[] VowelCounts { get; }
    }
}