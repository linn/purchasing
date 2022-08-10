namespace Linn.Purchasing.Service.Modules.Reports
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
            app.MapGet("/purchasing/reports/orders-by-supplier/export", this.GetOrdersBySupplierExport);
            app.MapGet("/purchasing/reports/orders-by-part/report", this.GetOrdersByPartReport);
            app.MapGet("/purchasing/reports/orders-by-part/export", this.GetOrdersByPartExport);
            app.MapGet("/purchasing/reports/suppliers-with-unacknowledged-orders", this.GetSuppliersWithUnacknowledgedOrdersReport);
            app.MapGet("/purchasing/reports/unacknowledged-orders", this.GetUnacknowledgedOrdersReport);
            app.MapGet("/purchasing/reports/unacknowledged-orders/export", this.GetUnacknowledgedOrdersReportExport);
            app.MapGet("/purchasing/reports/delivery-performance-summary", this.GetApp);
            app.MapGet("/purchasing/reports/delivery-performance-summary/report", this.GetDeliveryPerformanceSummaryReport);
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

        private async Task GetUnacknowledgedOrdersReportExport(
            HttpRequest request,
            HttpResponse response,
            IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService,
            int? supplierId,
            int? supplierGroupId,
            string name)
        {
            var resource = new UnacknowledgedOrdersRequestResource
                               {
                                   SupplierId = supplierId,
                                   SupplierGroupId = supplierGroupId,
                                   Name = name
                               };

            var csv = purchaseOrderReportFacadeService.GetUnacknowledgedOrdersReportExport(
                resource);

            var fileName = $"Unacknowledged purchase orders for {resource.Name}.csv";
            if (resource.SupplierId.HasValue)
            {
                fileName += $" ({resource.SupplierId}).csv";
            }

            await response.FromCsv(csv, fileName);
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

        private async Task GetOrdersByPartExport(
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
                                   PartNumber = partNumber, From = fromDate, To = toDate, Cancelled = cancelled
                               };

            var csv = purchaseOrderReportFacadeService.GetOrdersByPartExport(
                resource);

            await res.FromCsv(csv, $"ordersByPart{resource.From.Substring(0, 10)}_To_{resource.To.Substring(0, 10)}.csv");
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

        private async Task GetOrdersBySupplierExport(
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

            var csv = purchaseOrderReportFacadeService.GetOrdersBySupplierExport(
                resource);

            await res.FromCsv(csv, $"ordersBySupplier{resource.From.Substring(0, 10)}_To_{resource.To.Substring(0, 10)}.csv");
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
