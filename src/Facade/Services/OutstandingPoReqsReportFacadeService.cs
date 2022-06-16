namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;

    public class OutstandingPoReqsReportFacadeService : IOutstandingPoReqsReportFacadeService
    {
        private readonly IOutstandingPoReqsReportService domainService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public OutstandingPoReqsReportFacadeService(
            IOutstandingPoReqsReportService domainService,
            IReportReturnResourceBuilder resourceBuilder)
        {
            this.domainService = domainService;
            this.resourceBuilder = resourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(string state)
        {
            var result = this.domainService.GetReport(state);
            var resource = this.resourceBuilder.Build(new List<ResultsModel> { result });

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
