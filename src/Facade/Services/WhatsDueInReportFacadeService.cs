namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    public class WhatsDueInReportFacadeService : IWhatsDueInReportFacadeService
    {
        private readonly IWhatsDueInReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public WhatsDueInReportFacadeService(
            IWhatsDueInReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(
            string fromDate,
            string toDate,
            string orderBy,
            string vendorManager,
            int? supplier)
        {
            var resource = (ReportReturnResource)this.resultsModelResourceBuilder.Build(
                new List<ResultsModel>
                    {
                        this.domainService.GetReport(
                            DateTime.Parse(fromDate), DateTime.Parse(toDate), orderBy, vendorManager, supplier)
                    });

            return new SuccessResult<ReportReturnResource>(resource);
        }

        public IEnumerable<IEnumerable<string>> GetReportCsv(
            string fromDate, string toDate, string orderBy, string vendorManager, int? supplier)
        {
            return this.domainService.GetReport(
                DateTime.Parse(fromDate),
                DateTime.Parse(toDate),
                orderBy,
                vendorManager,
                supplier).ConvertToCsvList();
        }
    }
}
