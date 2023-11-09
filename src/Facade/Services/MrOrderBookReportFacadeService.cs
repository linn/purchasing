namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class MrOrderBookReportFacadeService : IMrOrderBookReportFacadeService
    {
        private readonly IMrOrderBookReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public MrOrderBookReportFacadeService(
            IMrOrderBookReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(int supplierId)
        {
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetOrderBookReport(supplierId));
            
            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IResult<ReportReturnResource> GetExportReport(int supplierId)
        {
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetOrderBookExport(supplierId));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
