﻿namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.Extensions;

    using Linn.Common.Reporting.Resources.ReportResultResources;
    using Linn.Common.Serialization;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

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

        public IResult<ReportReturnResource> GetOrdersByPartReport(
        OrdersByPartSearchResource resource,
        IEnumerable<string> privileges)
        {
            var fromValid = DateTime.TryParse(resource.From, out var from);
            var toValid = DateTime.TryParse(resource.To, out var to);

            if (!fromValid || !toValid)
            {
                return new BadRequestResult<ReportReturnResource>(
                    "Invalid dates supplied to orders by part report");
            }

            var cancelled = resource.Cancelled == "Y";

            var results = this.domainService.GetOrdersByPartReport(
                from,
                to,
                resource.PartNumber,
                cancelled);

            var returnResource = this.BuildResource(results, privileges);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IEnumerable<IEnumerable<string>> GetOrdersByPartExport(
            OrdersByPartSearchResource resource,
            IEnumerable<string> privileges)
        {
            var fromValid = DateTime.TryParse(resource.From, out var from);
            var toValid = DateTime.TryParse(resource.To, out var to);

            if (!fromValid || !toValid)
            {
                throw new Exception("Invalid dates supplied to orders by part export");
            }

            var cancelled = resource.Cancelled == "Y";

            var results = this.domainService.GetOrdersByPartReport(
                from,
                to,
                resource.PartNumber,
                cancelled);

            return results.ConvertToCsvList();
        }

        public IResult<ReportReturnResource> GetSuppliersWithUnacknowledgedOrdersReport(
            SuppliersWithUnacknowledgedOrdersRequestResource resource,
            IEnumerable<string> privileges)
        {
            var results = this.domainService.GetSuppliersWithUnacknowledgedOrders(
                resource.Planner,
                resource.VendorManager,
                resource.UseSupplierGroup);

            var returnResource = this.BuildResource(results, privileges);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IResult<ReportReturnResource> GetUnacknowledgedOrdersReport(
            UnacknowledgedOrdersRequestResource resource,
            IEnumerable<string> privileges)
        {
            var results = this.domainService.GetUnacknowledgedOrders(resource.SupplierId, resource.SupplierGroupId);

            var returnResource = this.BuildResource(results, privileges);

            return new SuccessResult<ReportReturnResource>(returnResource);
        }

        public IEnumerable<IEnumerable<string>> GetUnacknowledgedOrdersReportExport(UnacknowledgedOrdersRequestResource resource, IEnumerable<string> privileges)
        {
            var results = this.domainService.GetUnacknowledgedOrders(resource.SupplierId, resource.SupplierGroupId);

            return results.ConvertToCsvList();
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

        public IEnumerable<IEnumerable<string>> GetOrdersBySupplierExport(
            OrdersBySupplierSearchResource resource,
            IEnumerable<string> privileges)
        {
            var fromValid = DateTime.TryParse(resource.From, out var from);
            var toValid = DateTime.TryParse(resource.To, out var to);

            if (!fromValid || !toValid)
            {
                throw new Exception("Invalid dates supplied to orders by supplier export");
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

            return results.ConvertToCsvList();
        }

        private ReportReturnResource BuildResource(
            ResultsModel resultsModel, 
            IEnumerable<string> privileges)
        {
            return (ReportReturnResource)this.resultsModelResourceBuilder.Build(resultsModel, privileges);
        }
    }
}
