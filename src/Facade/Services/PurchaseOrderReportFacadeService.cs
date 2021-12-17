﻿namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderReportFacadeService : IPurchaseOrderReportFacadeService
    {
        private readonly IPurchaseOrdersReportService domainService;

        private readonly IBuilder<ResultsModel> resultsModelResourceBuilder;

        public PurchaseOrderReportFacadeService(
            IPurchaseOrdersReportService domainService,
            IBuilder<ResultsModel> resultsModelResourceBuilder)
        {
            this.domainService = domainService;
            this.resultsModelResourceBuilder = resultsModelResourceBuilder;
        }

        public IResult<ReportReturnResource> GetOrdersBySupplierReport(
            OrdersBySupplierSearchResource resource,
            IEnumerable<string> privileges)
        {
            var fromValid = DateTime.TryParse(resource.From, out var from);
            var toValid = DateTime.TryParse(resource.To, out var to);

            if (!fromValid || !toValid)
            {
                return new BadRequestResult<ReportReturnResource>(
                    "Invalid dates supplied to orders by supplier report");
            }

            var returns = resource.Returns == "Y";
            var outstanding = resource.Outstanding == "Y";
            var cancelled = resource.Cancelled == "Y";

            var results = this.domainService.GetOrdersBySupplierReport(
                from,
                to,
                resource.SupplierId,
                returns,
                outstanding,
                cancelled,
                resource.Credits,
                resource.StockControlled);

            var returnResource = this.BuildResource(results, privileges);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        private ReportReturnResource BuildResource(ResultsModel resultsModel, IEnumerable<string> privileges)
        {
            return (ReportReturnResource) this.resultsModelResourceBuilder.Build(resultsModel, privileges);
        }
    }
}
