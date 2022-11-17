namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomTreeService
    {
        BomTreeNode BuildBomTree(
            string bomName, 
            int? levels = null, 
            bool requirementOnly = true,
            bool showChanges = false);

        IEnumerable<BomTreeNode> FlattenBomTree(
            string bomName, 
            int? levels = null, 
            bool requirementOnly = true,
            bool showChanges = false);

        BomTreeNode BuildWhereUsedTree(
            string partNumber,
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false);

        IEnumerable<BomTreeNode> FlattenWhereUsedTree(
            string partNumber,
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false);
    }
}
