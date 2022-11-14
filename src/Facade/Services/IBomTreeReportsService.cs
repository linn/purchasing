namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Resources.Boms;

    public interface IBomTreeReportsService
    {
        IResult<BomTreeNodeResource> GetBomTree(string bomName, int levels);
    }
}
