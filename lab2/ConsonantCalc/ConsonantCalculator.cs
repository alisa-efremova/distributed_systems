using System;
using System.Linq;
using System.Threading;

namespace ConsonantCalc
{
    static class ConsonantCalculator
    {
        const string _consonants = "bcdfghjklmnpqrstvwxz";

        public static int[] GetConsonantCountPerLine(string[] textLines)
        {
            Thread.Sleep(1000);
            if (textLines == null || textLines.Length == 0)
            {
                return new int[] { };
            }

            int[] consonantCounts = new int[textLines.Length];
            for (int i = 0; i < textLines.Length; i++)
            {
                consonantCounts[i] = textLines[i].Count(c => _consonants.Contains(Char.ToLower(c)));
            }

            return consonantCounts;
        }
    }
}
