namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    public interface IBoardFileReadStrategy
    {
        (IList<BoardComponent>, string) ReadFile(string boardFile);
    }
}
