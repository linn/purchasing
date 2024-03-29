﻿namespace Linn.Purchasing.Facade.Services
{
    using System;

    using Linn.Common.Facade;
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
            var resource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetReport(
                    DateTime.Parse(fromDate).Date, DateTime.Parse(toDate).Date.AddDays(1).AddTicks(-1), orderBy, vendorManager, supplier));

            return new SuccessResult<ReportReturnResource>(resource);
        }
    }
}
