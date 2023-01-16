namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    public interface ICircuitBoardService
    {
        CircuitBoard UpdateComponents(
            string boardCode,
            PcasChange pcasChange,
            IEnumerable<BoardComponent> componentsToAdd,
            IEnumerable<BoardComponent> componentsToRemove);
    }
}
