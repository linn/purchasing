namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;

    public class PartsReceivedReportFacadeService : IPartsReceivedReportFacadeService
    {
        private readonly IPartsReceivedReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public PartsReceivedReportFacadeService(
            IPartsReceivedReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(PartsReceivedReportRequestResource options)
        {
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetReport(
                    options.Jobref,
                    options.Supplier,
                    options.FromDate,
                    options.ToDate,
                    options.OrderBy,
                    options.IncludeNegativeValues));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
