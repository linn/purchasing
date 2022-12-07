namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomChangeService
    {
        BomChange CreateBomChange(BomTreeNode tree, int changeRequestNumber, int enteredBy);

        BomTreeNode CreateBom(BomTreeNode tree, int changeRequestNumber, int changeId);
    }
}
