namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomTreeService
    {
        BomTreeNode BuildTree(string bomName, int? levels = null);

        IEnumerable<BomTreeNode> FlattenTree(string bomName, int? levels = null);
    }
}
