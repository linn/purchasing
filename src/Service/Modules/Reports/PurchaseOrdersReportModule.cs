﻿namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade.Carter.Extensions;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PurchaseOrdersReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/orders-by-supplier", this.GetApp);
            app.MapGet("/purchasing/reports/orders-by-part", this.GetApp);
            app.MapGet("/purchasing/reports/orders-by-supplier/report", this.GetOrdersBySupplierReport);
            app.MapGet("/purchasing/reports/orders-by-part/report", this.GetOrdersByPartReport);
            app.MapGet("/purchasing/reports/suppliers-with-unacknowledged-orders", this.GetSuppliersWithUnacknowledgedOrdersReport);
            app.MapGet("/purchasing/reports/unacknowledged-orders", this.GetUnacknowledgedOrdersReport);
            app.MapGet("/purchasing/reports/delivery-performance-summary", this.GetApp);
            app.MapGet("/purchasing/reports/delivery-performance-summary/report", this.GetDeliveryPerformanceSummaryReport);
            app.MapGet("/purchasing/reports/delivery-performance-supplier/report", this.GetDeliveryPerformanceBySupplierReport);
            app.MapGet("/purchasing/reports/delivery-performance-details/report", this.GetDeliveryPerformanceDetailsReport);
        }

        private async Task GetUnacknowledgedOrdersReport(
            HttpRequest request,
            HttpResponse response,
            int? supplierId,
            int? supplierGroupId,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService)
        {
            var resource = new UnacknowledgedOrdersRequestResource
                               {
                                   SupplierId = supplierId,
                                   SupplierGroupId = supplierGroupId
                               };

            var results = purchaseOrderReportFacadeService.GetUnacknowledgedOrdersReport(
                resource);

            await response.Negotiate(results);
        }

        private async Task GetDeliveryPerformanceSummaryReport(
            HttpRequest request,
            HttpResponse response,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService,
            int startPeriod,
            int endPeriod, 
            int? supplierId,
            string vendorManager)
        {
            var resource = new DeliveryPerformanceRequestResource
                               {
                                   StartPeriod = startPeriod, EndPeriod = endPeriod, SupplierId = supplierId, VendorManager = vendorManager
                               };
            var results = purchaseOrderReportFacadeService.GetDeliveryPerformanceSummaryReport(resource);

            await response.Negotiate(results);
        }

        private async Task GetDeliveryPerformanceBySupplierReport(
            HttpRequest request,
            HttpResponse response,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService,
            int startPeriod,
            int endPeriod,
            int? supplierId,
            string vendorManager)
        {
            var resource = new DeliveryPerformanceRequestResource
                               {
                                   StartPeriod = startPeriod,
                                   EndPeriod = endPeriod,
                                   SupplierId = supplierId,
                                   VendorManager = vendorManager
                               };
            var results = purchaseOrderReportFacadeService.GetDeliveryPerformanceSupplierReport(resource);

            await response.Negotiate(results);
        }

        private async Task GetDeliveryPerformanceDetailsReport(
            HttpRequest request,
            HttpResponse response,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService,
            int startPeriod,
            int endPeriod,
            int supplierId)
        {
            var resource = new DeliveryPerformanceRequestResource
                               {
                                   StartPeriod = startPeriod,
                                   EndPeriod = endPeriod,
                                   SupplierId = supplierId
                               };
            var results = purchaseOrderReportFacadeService.GetDeliveryPerformanceDetailReport(resource);

            await response.Negotiate(results);
        }

        private async Task GetSuppliersWithUnacknowledgedOrdersReport(
            HttpRequest request,
            HttpResponse response,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService,
            string vendorManager,
            int? planner,
            bool? useSupplierGroup)
        {
            var resource = new SuppliersWithUnacknowledgedOrdersRequestResource
                               {
                                   VendorManager = vendorManager,
                                   Planner = planner, 
                                   UseSupplierGroup = useSupplierGroup ?? false
                               };

            var results = purchaseOrderReportFacadeService.GetSuppliersWithUnacknowledgedOrdersReport(
                resource);

            await response.Negotiate(results);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }


        private async Task GetOrdersByPartReport(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService,
            string partNumber,
            string fromDate,
            string toDate,
            string cancelled)
        {
            var resource = new OrdersByPartSearchResource
                               {
                                   PartNumber = partNumber,
                                   From = fromDate,
                                   To = toDate,
                                   Cancelled = cancelled
                               };

            var results = purchaseOrderReportFacadeService.GetOrdersByPartReport(
                resource);

            await res.Negotiate(results);
        }

        private async Task GetOrdersBySupplierReport(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService,
            int id,
            string fromDate,
            string toDate,
            string returns,
            string outstanding,
            string cancelled,
            string credits,
            string stockControlled)
        {
            var resource = new OrdersBySupplierSearchResource
                               {
                                   SupplierId = id,
                                   From = fromDate,
                                   To = toDate,
                                   Returns = returns,
                                   Outstanding = outstanding,
                                   Cancelled = cancelled,
                                   Credits = credits,
                                   StockControlled = stockControlled
                               };

            var results = purchaseOrderReportFacadeService.GetOrdersBySupplierReport(
                resource);

            await res.Negotiate(results);
        }
    }
}
