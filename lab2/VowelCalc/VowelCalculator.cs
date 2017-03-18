using System;
using System.Linq;
using System.Threading;

namespace VowelCalc
{
    static class VowelCalculator
    {
        const string _vowels = "aeiouy";

        public static int[] GetVowelCountPerLine(string[] textLines)
        {
            Thread.Sleep(1000);
            if (textLines == null)
            {
                return new int[] {};
            }

            int[] vowelCounts = new int[textLines.Length];

            for (int i = 0; i < textLines.Length; i++)
            {
                vowelCounts[i] = textLines[i].Count(c => _vowels.Contains(Char.ToLower(c)));
            }

            return vowelCounts;
        }
    }
}
