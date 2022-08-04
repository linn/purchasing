namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;

    public class LeadTimesReportFacadeService : ILeadTimesReportFacadeService 
    {
        private readonly ILeadTimesReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public LeadTimesReportFacadeService(
            ILeadTimesReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetLeadTimesWithSupplierReport(LeadTimesReportRequestResource options)
        {
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetLeadTimesBySupplier(options.Supplier));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
