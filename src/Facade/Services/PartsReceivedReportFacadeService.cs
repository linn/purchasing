﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;


    public class PartsReceivedReportFacadeService : IPartsReceivedReportFacadeService
    {
        private readonly IPartsReceivedReportService domainService;

        private readonly IBuilder<IEnumerable<ResultsModel>> resultsModelResourceBuilder;

        public PartsReceivedReportFacadeService(
            IPartsReceivedReportService domainService,
            IBuilder<IEnumerable<ResultsModel>> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetReport(PartsReceivedReportRequestResource options)
        {
            var resource = (ReportReturnResource)this.resultsModelResourceBuilder.Build(
                new List<ResultsModel> 
                    {
                        this.domainService.GetReport(
                            options.Jobref,
                            options.Supplier,
                            options.FromDate,
                            options.ToDate,
                            options.OrderBy,
                            options.IncludeNegativeValues)
                    },
                null);

                                               return new SuccessResult<ReportReturnResource>(resource);
                                           }

        public IEnumerable<IEnumerable<string>> GetReportCsv(PartsReceivedReportRequestResource options)
        {
            return this.domainService.GetReport(
                options.Jobref,
                options.Supplier,
                options.FromDate,
                options.ToDate,
                options.OrderBy,
                options.IncludeNegativeValues).ConvertToCsvList();
        }
    }
}
