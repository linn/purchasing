namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomTreeService
    {
        BomTreeNode BuildTree(string bomName, int? levels = null);
    }
}
