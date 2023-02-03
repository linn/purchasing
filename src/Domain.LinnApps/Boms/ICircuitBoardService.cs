namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    public interface ICircuitBoardService
    {
        CircuitBoard UpdateComponents(
            string boardCode,
            PcasChange pcasChange,
            int changeRequestId,
            IEnumerable<BoardComponent> componentsToAdd,
            IEnumerable<BoardComponent> componentsToRemove);

        ProcessResult UpdateFromFile(
            string boardCode,
            string revisionCode,
            string fileType,
            string fileString,
            bool makeChanges);
    }
}
