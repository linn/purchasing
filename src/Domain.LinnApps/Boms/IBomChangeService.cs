namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomChangeService
    {
        BomTreeNode CreateBomChanges(BomTreeNode tree, int changeRequestNumber, int enteredBy);

        void CopyBom(string srcPartNumber, string destBomPartNumber, int changedBy, int crfNumber);
    }
}
