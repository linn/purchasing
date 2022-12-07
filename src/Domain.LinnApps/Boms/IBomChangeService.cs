namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomChangeService
    {
        IEnumerable<BomChange> CreateBomChanges(BomTreeNode tree, int changeRequestNumber, int enteredBy);

        BomTreeNode CreateBom(BomTreeNode tree, int changeRequestNumber, int changeId);
    }
}
