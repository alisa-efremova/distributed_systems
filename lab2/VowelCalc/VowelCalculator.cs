using System;
using System.Linq;

namespace VowelCalc
{
    static class VowelCalculator
    {
        const string _vowels = "aeiou";

        public static int[] GetVowelCountPerLine(string text)
        {
            if (text == null || text == string.Empty)
            {
                return new int[] {};
            }

            string[] lines = text.Split('\n');
            int[] vowelCounts = new int[lines.Length];

            for(int i = 0; i < lines.Length; i++)
            {
                vowelCounts[i] = lines[i].Count(c => _vowels.Contains(Char.ToLower(c)));
            }

            return vowelCounts;
        }
    }
}
