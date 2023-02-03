namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class TabSeparatedReadStrategy : IBoardFileReadStrategy
    {
        private static readonly char[] CheckArray = "0123456789".ToCharArray();

        public (IList<BoardComponent>, string) ReadFile(string boardFile)
        {
            var components = new List<BoardComponent>();
            var pcbPartNumber = string.Empty;
            var reader = new StringReader(boardFile);
            while (reader.ReadLine() is { } line)
            {
                if (line.StartsWith("Designator") || string.IsNullOrWhiteSpace(line)) continue;
                var items = line.Split('\t').Select(a => a.Trim('"')).ToList();
                if (!items[1].StartsWith("PCB") && !string.IsNullOrWhiteSpace(items[1]))
                {
                    components.Add(new BoardComponent { CRef = items[0], PartNumber = this.PadPartNumber(items[1]) });
                }
                else
                {
                    pcbPartNumber = this.PadPartNumber(items[1]);
                }
            }

            return (components, pcbPartNumber);
        }

        private string PadPartNumber(string partNumber)
        {
            if (partNumber.Contains(' ') || partNumber.Length == 14)
            {
                return partNumber;
            }

            var firstNumberIndex = partNumber.IndexOfAny(CheckArray);
            return firstNumberIndex == -1 ? partNumber : partNumber.Insert(firstNumberIndex, " ");
        }
    }
}
