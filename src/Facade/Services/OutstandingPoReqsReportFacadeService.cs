namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class OutstandingPoReqsReportFacadeService : IOutstandingPoReqsReportFacadeService
    {
        private readonly IOutstandingPoReqsReportService domainService;

        private readonly IBuilder<ResultsModel> resourceBuilder;

        public OutstandingPoReqsReportFacadeService(
            IOutstandingPoReqsReportService domainService,
            IBuilder<ResultsModel> resourceBuilder)
        {
            this.domainService = domainService;
            this.resourceBuilder = resourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(string state)
        {
            var result = this.domainService.GetReport(state);
            var resource = (ReportReturnResource)this.resourceBuilder.Build(result, null);

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
