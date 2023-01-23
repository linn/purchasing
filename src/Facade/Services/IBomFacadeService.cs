namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public interface IBomFacadeService
    {
        IResult<BomTreeNode> PostBom(PostBomResource node);

        IResult<BomTreeNode> CopyBom(
            string srcPartNumber, string destPartNumber, int changedBy, int crfNumber);

        IResult<BomTreeNode> DeleteBom(string bomName, int crfNumber, int changedBy);

        IResult<BomTreeNode> ExplodeSubAssembly(
            string bomName, int crfNumber, string subAssembly, int changedBy);
    }
}
