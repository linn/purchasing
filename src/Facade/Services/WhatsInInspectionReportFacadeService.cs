namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;

    public class WhatsInInspectionReportFacadeService : IWhatsInInspectionReportFacadeService
    {
        private readonly IWhatsInInspectionReportService domainService;

        public WhatsInInspectionReportFacadeService(IWhatsInInspectionReportService domainService)
        {
            this.domainService = domainService;
        }

        public IResult<WhatsInInspectionReportResource> GetReport(
            bool includePartsWithNoOrderNumber = false,
            bool showStockLocations = true,
            bool includeFailedStock = false,
            bool excludeFinishedGoods = false,
            bool showBackOrdered = true)
        {
            var test = this.domainService.GetReport();
            throw new System.NotImplementedException();
        }
    }
}
