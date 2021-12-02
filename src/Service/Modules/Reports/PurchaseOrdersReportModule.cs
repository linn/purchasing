namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
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
            var supplierId = req.RouteValues.As<int>("id");
            var resource = await req.Bind<OrdersBySupplierSearchResource>();
            resource.SupplierId = supplierId;

            resource.From = "2/11/21";
            resource.To = "2/12/21";

            var results = this.purchaseOrderReportFacadeService.GetOrdersBySupplierReport(resource);

            await res.Negotiate(results);
        }
    }
}
