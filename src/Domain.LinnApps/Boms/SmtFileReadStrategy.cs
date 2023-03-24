namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Linn.Purchasing.Domain.LinnApps.Boms.Extensions;

    public class SmtFileReadStrategy : IBoardFileReadStrategy
    {
        public (IList<BoardComponent>, string) ReadFile(string boardFile)
        {
            var components = new List<BoardComponent>();
            var reader = new StringReader(boardFile);
            while (reader.ReadLine() is { } line)
            {
                if (!line.TrimStart().StartsWith("#C")) continue;

                var items = line.TrimStart().Substring(2).Split(';').Select(a => a.Trim()).ToList();

                if (!string.IsNullOrWhiteSpace(items[0]))
                {
                    components.Add(new BoardComponent { CRef = items[1], PartNumber = items[0].PadPartNumber(), Quantity = 1 });
                }
            }

            return (components, null);
        }
    }
}
