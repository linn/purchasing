namespace Linn.Purchasing.Facade.Services
{
    using System;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class PrefSupReceiptsReportFacadeService : IPrefSupReceiptsReportFacadeService
    {
        private readonly IPrefSupReceiptsReportService domainService;

        private readonly IReportReturnResourceBuilder resourceBuilder;

        public PrefSupReceiptsReportFacadeService(IPrefSupReceiptsReportService domainService, IReportReturnResourceBuilder resourceBuilder)
        {
            this.domainService = domainService;
            this.resourceBuilder = resourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(string fromDate, string toDate)
        {
            var fromValid = DateTime.TryParse(fromDate, out var from);
            var toValid = DateTime.TryParse(toDate, out var to);

            if (!fromValid || !toValid)
            {
                return new BadRequestResult<ReportReturnResource>(
                    "Invalid dates supplied to pref sup receipts report");
            }

            var result = this.domainService.GetReport(from, to);
            var resource = this.resourceBuilder.Build(result);

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
