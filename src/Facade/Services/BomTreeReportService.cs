namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomTreeReportService : IBomTreeReportsService
    {
        private readonly IBomTreeService domainService;

        public BomTreeReportService(
           IBomTreeService domainService)
        {
           this.domainService = domainService;
        }

        public IResult<BomTreeNode> GetBomTree(string bomName, int? levels = null)
        {
            return new SuccessResult<BomTreeNode>(this.domainService.BuildTree(bomName, levels));
        }
    }
}
