namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;

    public interface IBomFacadeService
    {
        IResult<BomTreeNode> PostBom(PostBomResource node);

        IResult<ProcessResultResource> CopyBom(
            string srcPartNumber, string destPartNumber, int changedBy, int crfNumber);
    }
}
