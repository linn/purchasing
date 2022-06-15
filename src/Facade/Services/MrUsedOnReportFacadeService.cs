namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    public class MrUsedOnReportFacadeService : IMrUsedOnReportFacadeService
    {
        private readonly IMrUsedOnReportService reportService;

        private readonly IBuilder<ResultsModel> resultsModelResourceBuilder;

        public MrUsedOnReportFacadeService(
            IMrUsedOnReportService reportService,
            IBuilder<ResultsModel> resultsModelResourceBuilder)
        {
            this.reportService = reportService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(string partNumber, string jobRef)
        {
            var resource = (ReportReturnResource)this.resultsModelResourceBuilder.Build(
                this.reportService.GetUsedOn(partNumber, jobRef),
                null);

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
