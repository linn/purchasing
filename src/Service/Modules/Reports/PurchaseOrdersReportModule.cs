namespace Linn.Purchasing.Service.Modules.Reports
{
    using System;
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
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    public class PurchaseOrdersReportModule : CarterModule
    {
        private readonly IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService;

        public PurchaseOrdersReportModule(IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService)
        {
            this.purchaseOrderReportFacadeService = purchaseOrderReportFacadeService;
            this.Get("/purchasing/reports/orders-by-supplier", this.GetApp);
            this.Get("/purchasing/reports/orders-by-supplier/report", this.GetOrdersBySupplierReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetOrdersBySupplierReport(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<OrdersBySupplierSearchResource>();
            //will I just remove the await bind bit^?
            //It's not actually doing anything now that they're all query strings..

            StringValues id = StringValues.Empty;
            req.Query.TryGetValue("Id", out id);

            StringValues from = StringValues.Empty;
            req.Query.TryGetValue("FromDate", out from);

            StringValues to = StringValues.Empty;
            req.Query.TryGetValue("ToDate", out to);

            resource.SupplierId = int.Parse(id);
            resource.From = from;
            resource.To = to;

            var results = this.purchaseOrderReportFacadeService.GetOrdersBySupplierReport(resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(results);
        }
    }
}
