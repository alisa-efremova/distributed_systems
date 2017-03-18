using System;
using System.Configuration;
using System.Threading;
using System.Collections.Generic;

namespace BestLineSelector
{
    static class BestLinesAnalyzer
    {
        const double _maxDifferenceAllowed = 0.1;

        static public string[] ExtractBestLines(string[] textLines, int[] vowelsCount, int[] consonantCount)
        {
            Thread.Sleep(1000);
            if (textLines == null || textLines.Length == 0)
            {
                return textLines;
            }

            double ratio = GetOptimalVowelsToConsonantsRatio();
            Console.WriteLine("Expected ratio:" + ratio);

            List<string> bestLines = new List<string>();
            for (int i = 0; i < textLines.Length; i++)
            {
                if (consonantCount[i] != 0)
                {
                    double currentRatio = (double)vowelsCount[i]/(double)consonantCount[i];
                    Console.WriteLine(i + ": " + currentRatio);
                    if (Math.Abs(currentRatio - ratio) < _maxDifferenceAllowed)
                    {
                        bestLines.Add(textLines[i]);
                    }
                }
            }

            return bestLines.ToArray();
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
