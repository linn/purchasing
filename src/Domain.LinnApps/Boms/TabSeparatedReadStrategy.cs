namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.Boms.Extensions;

    public class TabSeparatedReadStrategy : IBoardFileReadStrategy
    {
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
                    components.Add(new BoardComponent { CRef = items[0].PadCRef(), PartNumber = items[1].PadPartNumber(), Quantity = 1 });
                }
                else
                {
                    pcbPartNumber = items[1].PadPartNumber();
                }
            }

            return (components, pcbPartNumber);
        }
    }
}
