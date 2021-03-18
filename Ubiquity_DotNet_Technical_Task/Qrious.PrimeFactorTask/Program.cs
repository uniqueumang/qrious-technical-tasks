using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Qrious.PrimeFactorTask
{
    internal class Program
    {
        private static readonly HashSet<ulong> Unique12DigitNumberSet = Get12DigitUniqueSequencialNumbers();

        private static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();

            var results = Unique12DigitNumberSet.Select(FindFourPrimeFactorsOfGivenNumber).Where(t => t != null);

            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Console.WriteLine($"Time Taken in ms: {stopwatch.ElapsedMilliseconds}");
        }

        private static ResultModel FindFourPrimeFactorsOfGivenNumber(ulong twelveDigitNumber)
        {
            var primeNumbers = PrimeNumber.GetFourPrimeNumbers(twelveDigitNumber);

            if (primeNumbers != null)
            {
                return new ResultModel(primeNumbers.ToArray(), twelveDigitNumber);
            }

            return null;
        }

        /// <summary>
        /// Get all unique 12-digit numbers such that each digit is either same as last one or sequencial in ascending order. 
        /// </summary>
        /// <returns></returns>
        private static HashSet<ulong> Get12DigitUniqueSequencialNumbers()
        {
            var listOfNumber = new ulong[] {1, 2, 3, 4, 5, 6, 7, 8, 9};
            var listOfPossibleNumber = new HashSet<ulong>();

            foreach (var number in listOfNumber)
            {
                var node = new Node(number);
                var values = node.GetDataOfAllLeaf();
                listOfPossibleNumber.UnionWith(values);
            }

            return listOfPossibleNumber;
        }
    }
}