namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomChangeService
    {
        BomTreeNode ProcessBomChanges(BomTreeNode tree);
    }
}
