namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class TabSeparatedReadStrategy : IBoardFileReadStrategy
    {
        public IEnumerable<BoardComponent> ReadFile(string boardFile)
        {
            var components = new List<BoardComponent>();
            var reader = new StringReader(boardFile);
            while (reader.ReadLine() is { } line)
            {
                if (line.StartsWith("Designator") || string.IsNullOrWhiteSpace(line)) continue;
                var items = line.Split('\t').Select(a => a.Trim('"')).ToList();
                components.Add(new BoardComponent { CRef = items[0], PartNumber = items[1] });
            }

            return components;
        }
    }
}
