namespace Linn.Purchasing.Domain.LinnApps.Boms.Extensions
{
    public static class FileReaderExtensions
    {
        private static readonly char[] CheckArray = "0123456789".ToCharArray();

        public static string PadPartNumber(this string partNumberText)
        {
            if (partNumberText.Contains(' ') || partNumberText.Length == 14 || string.IsNullOrWhiteSpace(partNumberText))
            {
                return partNumberText;
            }

            var firstNumberIndex = partNumberText.IndexOfAny(CheckArray);
            return firstNumberIndex == -1 ? partNumberText : partNumberText.Insert(firstNumberIndex, " ");
        }

        public static string PadCRef(this string crefText)
        {
            if (string.IsNullOrWhiteSpace(crefText))
            {
                return crefText;
            }

            var firstNumberIndex = crefText.IndexOfAny(CheckArray);
            var numberLength = crefText.Length - firstNumberIndex;
            if (numberLength >= 3)
            {
                return crefText;
            }
            
            var paddedText = crefText;
            for (var i = numberLength; i < 3; i++)
            {
                paddedText = paddedText.Insert(firstNumberIndex, "0");
            }

            return paddedText;
        }
    }
}
