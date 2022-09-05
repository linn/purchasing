namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class ForecastWeekChangesFacadeService : IForecastWeekChangesFacadeService
    {
        private readonly IForecastWeekChangesReportService domainService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public ForecastWeekChangesFacadeService(
            IForecastWeekChangesReportService domainService,
            IReportReturnResourceBuilder resourceBuilder)
        {
            this.domainService = domainService;
            this.resourceBuilder = resourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport()
        {
            return new SuccessResult<ReportReturnResource>(
                this.resourceBuilder.Build(this.domainService.GetReport()));
        }
    }
}
