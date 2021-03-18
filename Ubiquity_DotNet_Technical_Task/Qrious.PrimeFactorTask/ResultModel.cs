using System.Text;

namespace Qrious.PrimeFactorTask
{
    public class ResultModel
    {
        public uint[] PrimeNumbers { get; }
        public ulong TwelveDigitNumber { get; }

        public ResultModel(uint[] primeNumbers, ulong twelveDigitNumber)
        {
            PrimeNumbers = primeNumbers;
            TwelveDigitNumber = twelveDigitNumber;
        }

        public override string ToString()
        {
            var stringAppender = new StringBuilder($"12 digit Number: {TwelveDigitNumber} Prime Numbers: ");

            foreach (var primeNumber in PrimeNumbers)
            {
                stringAppender.Append($"{primeNumber} ");
            }

            return stringAppender.ToString();
        }
    }
}