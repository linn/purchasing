﻿namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.IO;
    using System.Net.Mime;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class PurchaseOrdersReportModule : CarterModule
    {
        private readonly IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService;

        public PurchaseOrdersReportModule(IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService)
        {
            this.purchaseOrderReportFacadeService = purchaseOrderReportFacadeService;
            this.Get("/purchasing/reports/orders-by-supplier", this.GetApp);
            this.Get("/purchasing/reports/orders-by-part", this.GetApp);
            this.Get("/purchasing/reports/orders-by-supplier/report", this.GetOrdersBySupplierReport);
            this.Get("/purchasing/reports/orders-by-supplier/export", this.GetOrdersBySupplierExport);
            this.Get("/purchasing/reports/orders-by-part/report", this.GetOrdersByPartReport);
            this.Get("/purchasing/reports/orders-by-part/export", this.GetOrdersByPartExport);
            this.Get("/purchasing/reports/suppliers-with-unacknowledged-orders", this.GetSuppliersWithUnacknowledgedOrdersReport);
            this.Get("/purchasing/reports/unacknowledged-orders", this.GetUnacknowledgedOrdersReport);
            this.Get("/purchasing/reports/unacknowledged-orders/export", this.GetUnacknowledgedOrdersReportExport);
        }

        private async Task GetUnacknowledgedOrdersReport(HttpRequest request, HttpResponse response)
        {
            var resource = new UnacknowledgedOrdersRequestResource
            {
                                   SupplierId = request.Query.As<int?>("SupplierId"),
                                   OrganisationId = request.Query.As<int?>("OrganisationId")
                               };

            var results = this.purchaseOrderReportFacadeService.GetUnacknowledgedOrdersReport(
                resource,
                request.HttpContext.GetPrivileges());

            await response.Negotiate(results);
        }

        private async Task GetUnacknowledgedOrdersReportExport(HttpRequest request, HttpResponse response)
        {
            var resource = new UnacknowledgedOrdersRequestResource
                               {
                                   SupplierId = request.Query.As<int?>("SupplierId"),
                                   OrganisationId = request.Query.As<int?>("OrganisationId"),
                                   Name = request.Query.As<string>("Name")
                               };

            var stream = this.purchaseOrderReportFacadeService.GetUnacknowledgedOrdersReportExport(
                                      resource,
                                      request.HttpContext.GetPrivileges());
            
            var contentDisposition = new ContentDisposition
                                         {
                                             FileName =
                                                 $"Unacknowledged purchase orders for {resource.Name} ({resource.SupplierId ?? resource.OrganisationId}).csv"
                                         };

            stream.Position = 0;
            await response.FromStream(stream, "text/csv", contentDisposition);
        }

        private async Task GetSuppliersWithUnacknowledgedOrdersReport(HttpRequest request, HttpResponse response)
        {
            var resource = new SuppliersWithUnacknowledgedOrdersRequestResource
                               {
                                   VendorManager = request.Query.As<string>("VendorManager"),
                                   Planner = request.Query.As<int?>("Planner")
                               };

            var results = this.purchaseOrderReportFacadeService.GetSuppliersWithUnacknowledgedOrdersReport(
                resource,
                request.HttpContext.GetPrivileges());

            await response.Negotiate(results);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetOrdersByPartExport(HttpRequest req, HttpResponse res)
        {
            var resource = new OrdersByPartSearchResource();
            resource.PartNumber = req.Query.As<string>("PartNumber");
            resource.From = req.Query.As<string>("FromDate");
            resource.To = req.Query.As<string>("ToDate");
            resource.Cancelled = req.Query.As<string>("Cancelled");

            using Stream stream = this.purchaseOrderReportFacadeService.GetOrdersByPartExport(
                resource,
                req.HttpContext.GetPrivileges());

            var contentDisposition = new ContentDisposition
                                         {
                                             FileName =
                                                 $"ordersByPart{resource.From.Substring(0, 10)}_To_{resource.To.Substring(0, 10)}.csv"
                                         };

            stream.Position = 0;
            await res.FromStream(stream, "text/csv", contentDisposition);
        }

        private async Task GetOrdersByPartReport(HttpRequest req, HttpResponse res)
        {
            var resource = new OrdersByPartSearchResource();
            resource.PartNumber = req.Query.As<string>("PartNumber");
            resource.From = req.Query.As<string>("FromDate");
            resource.To = req.Query.As<string>("ToDate");
            resource.Cancelled = req.Query.As<string>("Cancelled");

            var results = this.purchaseOrderReportFacadeService.GetOrdersByPartReport(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(results);
        }

        private async Task GetOrdersBySupplierExport(HttpRequest req, HttpResponse res)
        {
            var resource = new OrdersBySupplierSearchResource();
            resource.SupplierId = req.Query.As<int>("Id");
            resource.From = req.Query.As<string>("FromDate");
            resource.To = req.Query.As<string>("ToDate");

            resource.Returns = req.Query.As<string>("Returns");
            resource.Outstanding = req.Query.As<string>("Outstanding");
            resource.Cancelled = req.Query.As<string>("Cancelled");
            resource.Credits = req.Query.As<string>("Credits");
            resource.StockControlled = req.Query.As<string>("StockControlled");

            using Stream stream = this.purchaseOrderReportFacadeService.GetOrdersBySupplierExport(
                resource,
                req.HttpContext.GetPrivileges());

            var contentDisposition = new ContentDisposition
                                         {
                                             FileName =
                                                 $"ordersBySupplier{resource.From.Substring(0, 10)}_To_{resource.To.Substring(0, 10)}.csv"
                                         };

            stream.Position = 0;
            await res.FromStream(stream, "text/csv", contentDisposition);
        }

        private async Task GetOrdersBySupplierReport(HttpRequest req, HttpResponse res)
        {
            var resource = new OrdersBySupplierSearchResource();
            resource.SupplierId = req.Query.As<int>("Id");
            resource.From = req.Query.As<string>("FromDate");
            resource.To = req.Query.As<string>("ToDate");

            resource.Returns = req.Query.As<string>("Returns");
            resource.Outstanding = req.Query.As<string>("Outstanding");
            resource.Cancelled = req.Query.As<string>("Cancelled");
            resource.Credits = req.Query.As<string>("Credits");
            resource.StockControlled = req.Query.As<string>("StockControlled");

            var results = this.purchaseOrderReportFacadeService.GetOrdersBySupplierReport(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(results);
        }
    }
}
