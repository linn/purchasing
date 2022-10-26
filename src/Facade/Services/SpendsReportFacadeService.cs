﻿namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.Extensions;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources.RequestResources;

    public class SpendsReportFacadeService : ISpendsReportFacadeService
    {
        private readonly ISpendsReportService domainService;

        private readonly IReportReturnResourceBuilder resultsModelResourceBuilder;

        public SpendsReportFacadeService(
            ISpendsReportService domainService,
            IReportReturnResourceBuilder resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IEnumerable<IEnumerable<string>> GetSpendBySupplierExport(string vendorManagerId)
        {
            return this.domainService.GetSpendBySupplierReport(vendorManagerId).ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId)
        {
            var results = this.domainService.GetSpendBySupplierReport(vendorManagerId);

            var returnResource = this.resultsModelResourceBuilder.Build(results);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IEnumerable<IEnumerable<string>> GetSpendBySupplierByDateRangeReportExport(SpendBySupplierByDateRangeReportRequestResource options)
        {
            return this.domainService.GetSpendBySupplierByDateRangeReport(
                options.FromDate,
                options.ToDate,
                options.VendorManager,
                options.SupplierId).ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSpendBySupplierByDateRangeReport(SpendBySupplierByDateRangeReportRequestResource options)
        {
            var returnResource = this.resultsModelResourceBuilder.Build(
                this.domainService.GetSpendBySupplierByDateRangeReport(
                    options.FromDate,
                    options.ToDate,
                    options.VendorManager,
                    options.SupplierId));

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IEnumerable<IEnumerable<string>> GetSpendByPartExport(int supplierId)
        {
            return this.domainService.GetSpendByPartReport(supplierId).ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSpendByPartReport(int supplierId)
        {
            var results = this.domainService.GetSpendByPartReport(supplierId);

            var returnResource = this.resultsModelResourceBuilder.Build(results);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }
    }
}
