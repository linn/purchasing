namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    public class MrUsedOnReportFacadeService : IMrUsedOnReportFacadeService
    {
        private readonly IMrUsedOnReportService reportService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public MrUsedOnReportFacadeService(
            IMrUsedOnReportService reportService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.reportService = reportService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(string partNumber, string jobRef)
        {
            var resource = this.resultsModelResourceBuilder.Build(
                this.reportService.GetUsedOn(partNumber, jobRef));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
