using System;
using System.Configuration;

namespace BestLineSelector
{
    static class BestLinesAnalyzer
    {
        const double _maxDifferenceAllowed = 0.0001;

        static public string ExtractBestLines(string text, int[] vowelsCount, int[] consonantCount)
        {
            if (text == null || text == string.Empty)
            {
                return text;
            }

            double ratio = GetOptimalVowelsToConsonantsRatio();

            string[] lines = text.Split('\n');
            string result = "";

            for (int i = 0; i < lines.Length; i++)
            {
                if (Math.Abs(vowelsCount[i]/consonantCount[i] - ratio) < _maxDifferenceAllowed)
                {
                    result += lines[i] + '\n';
                }
            }

            return result;
        }

        static double GetOptimalVowelsToConsonantsRatio()
        {
            string[] ratio = ConfigurationManager.AppSettings["VowelsToConsonantsRatio"].Split(':');
            if (ratio.Length != 2)
            {
                throw new System.InvalidOperationException("Invalid ratio in AppSettings.");
            }

            int vowels = Int32.Parse(ratio[0]);
            int consonants = Int32.Parse(ratio[1]);
            return vowels/consonants;
        }
    }
}
