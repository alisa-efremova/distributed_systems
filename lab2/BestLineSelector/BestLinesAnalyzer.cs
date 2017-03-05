using System;
using System.Configuration;
using System.Threading;

namespace BestLineSelector
{
    static class BestLinesAnalyzer
    {
        const double _maxDifferenceAllowed = 0.0001;

        static public string ExtractBestLines(string text, int[] vowelsCount, int[] consonantCount)
        {
            Thread.Sleep(1000);
            if (text == null || text == string.Empty)
            {
                return text;
            }

            double ratio = GetOptimalVowelsToConsonantsRatio();
            Console.WriteLine("Expected ratio:" + ratio);

            string[] lines = text.Split('\n');
            string result = "";

            for (int i = 0; i < lines.Length; i++)
            {
                if (consonantCount[i] != 0)
                {
                    double currentRatio = (double)vowelsCount[i]/(double)consonantCount[i];
                    Console.WriteLine(i + ": " + currentRatio);
                    if (Math.Abs(currentRatio - ratio) < _maxDifferenceAllowed)
                    {
                        result += lines[i] + '\n';
                    }
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
            return (double)vowels/(double)consonants;
        }
    }
}
