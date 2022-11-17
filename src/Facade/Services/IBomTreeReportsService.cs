namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomTreeReportsService
    {
        IResult<BomTreeNode> GetBomTree(
            string bomName, 
            int? levels = null,
            bool requirementOnly = true,
            bool showChanges = false);

        IEnumerable<IEnumerable<string>> GetFlatBomTreeExport(
            string bomName, 
            int? levels,
            bool requirementOnly = true,
            bool showChanges = false);
    }
}
