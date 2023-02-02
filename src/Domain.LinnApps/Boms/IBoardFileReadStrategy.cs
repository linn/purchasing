namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    public interface IBoardFileReadStrategy
    {
        IEnumerable<BoardComponent> ReadFile(string boardFile);
    }
}
