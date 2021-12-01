namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class PurchaseOrdersReportModule : CarterModule
    {
        private readonly IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService;

        public PurchaseOrdersReportModule(IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService)
        {
            this.purchaseOrderReportFacadeService = purchaseOrderReportFacadeService;
            this.Get("/purchasing/reports/orders-by-supplier", this.GetApp);
            this.Get("/purchasing/reports/orders-by-supplier/{id:int}", this.GetOrdersBySupplierReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetOrdersBySupplierReport(HttpRequest req, HttpResponse res)
        {
            // var thingId = req.RouteValues.As<OrdersBySupplierSearchResource>();
            this.purchaseOrderReportFacadeService.GetOrdersBySupplierReport(new OrdersBySupplierSearchResource());

            await res.Negotiate(new SuccessResult<ProcessResultResource>(new ProcessResultResource(true, "ok")));
        }
    }
}
