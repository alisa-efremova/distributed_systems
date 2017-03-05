using System;
using System.Linq;
using System.Threading;

namespace ConsonantCalc
{
    static class ConsonantCalculator
    {
        const string _consonants = "bcdfghjklmnpqrstvwxz";

        public static int[] GetConsonantCountPerLine(string text)
        {
            Thread.Sleep(1000);
            if (text == null || text == string.Empty)
            {
                return new int[] { };
            }

            string[] lines = text.Split('\n');
            int[] consonantCounts = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                consonantCounts[i] = lines[i].Count(c => _consonants.Contains(Char.ToLower(c)));
            }

            return consonantCounts;
        }
    }
}
