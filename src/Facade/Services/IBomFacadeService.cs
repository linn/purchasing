namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomFacadeService
    {
        IResult<BomTreeNode> PostBom(BomTreeNode node);
    }
}
