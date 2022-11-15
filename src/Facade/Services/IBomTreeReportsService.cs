namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomTreeReportsService
    {
        IResult<BomTreeNode> GetBomTree(string bomName, int? levels = null);
    }
}
