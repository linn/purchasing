﻿namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
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

        public IResult<ReportReturnResource> GetSpendBySupplierReport(string vendorManagerId)
        {
            var results = this.domainService.GetSpendBySupplierReport(vendorManagerId);

            var returnResource = this.resultsModelResourceBuilder.Build(results);

            return new SuccessResult<ReportReturnResource>(returnResource);
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

        public IResult<ReportReturnResource> GetSpendByPartReport(int supplierId)
        {
            var results = this.domainService.GetSpendByPartReport(supplierId);

            var returnResource = this.resultsModelResourceBuilder.Build(results);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IResult<ReportReturnResource> GetSpendByPartByDateReport(SpendBySupplierByDateRangeReportRequestResource options)
        {
            if (options?.SupplierId is null)
            {
                return new BadRequestResult<ReportReturnResource>("You must supply a supplier id");
            }

            var results = this.domainService.GetSpendByPartByDateReport(options.SupplierId.Value, options.FromDate, options.ToDate);

            var returnResource = this.resultsModelResourceBuilder.Build(results);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }
    }
}
