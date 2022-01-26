namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.IO;
    using System.Net.Mime;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
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
            this.Get("/purchasing/reports/orders-by-supplier/report", this.GetOrdersBySupplierReport);
            this.Get("/purchasing/reports/orders-by-supplier/export", this.GetOrdersBySupplierExport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
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

            using MemoryStream stream = new();

            var result = this.purchaseOrderReportFacadeService.GetOrdersBySupplierExport(
                resource,
                req.HttpContext.GetPrivileges(), stream);

            if (!(result is SuccessResult<bool>))
            {
                await res.Negotiate(result);
            }

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
