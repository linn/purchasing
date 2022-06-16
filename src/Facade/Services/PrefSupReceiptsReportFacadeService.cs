namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;

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
            var resource = (ReportReturnResource)this.resourceBuilder.Build(
                new List<ResultsModel> { result });

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IEnumerable<IEnumerable<string>> GetExport(string fromDate, string toDate)
        {
            var fromValid = DateTime.TryParse(fromDate, out var from);
            var toValid = DateTime.TryParse(toDate, out var to);

            if (!fromValid || !toValid)
            {
                throw new Exception("Invalid dates supplied to pref sup receipts report");
            }

            var results = this.domainService.GetReport(from, to, true);

            return results.ConvertToCsvList();
        }
    }
}