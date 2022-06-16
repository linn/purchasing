namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    public class MrOrderBookReportService : IMrOrderBookReportService
    {
        private readonly IQueryRepository<MrPurchaseOrderDetail> repository;

        private readonly ISingleRecordRepository<MrMaster> mrMaster;

        public MrOrderBookReportService(
            IQueryRepository<MrPurchaseOrderDetail> repository,
            ISingleRecordRepository<MrMaster> mrMaster)
        {
            this.repository = repository;
            this.mrMaster = mrMaster;
        }

        public ResultsModel GetOrderBookReport(int supplierId)
        {
            var jobRef = this.mrMaster.GetRecord().JobRef;
            var data = this.repository.FilterBy(x => x.SupplierId == supplierId && x.JobRef == jobRef);
            throw new System.NotImplementedException();
        }
    }
}
