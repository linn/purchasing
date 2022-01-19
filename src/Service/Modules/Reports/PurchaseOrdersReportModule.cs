namespace Linn.Purchasing.Service.Modules.Reports
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Common.Serialization;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Primitives;

    public class PurchaseOrdersReportModule : CarterModule
    {
        private readonly IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService;

        private readonly CsvStreamWriter csvStreamWriter;

        private MemoryStream stream;

        public PurchaseOrdersReportModule(IPurchaseOrderReportFacadeService purchaseOrderReportFacadeService)
        {
            this.purchaseOrderReportFacadeService = purchaseOrderReportFacadeService;
            this.Get("/purchasing/reports/orders-by-supplier", this.GetApp);
            this.Get("/purchasing/reports/orders-by-supplier/report", this.GetOrdersBySupplierReport);
            this.Get("/purchasing/reports/orders-by-supplier/export", this.GetOrdersBySupplierExport);
            this.stream = new MemoryStream();
            this.csvStreamWriter = new CsvStreamWriter(this.stream);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
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
            
            var results = this.purchaseOrderReportFacadeService.GetOrdersBySupplierReport(resource, req.HttpContext.GetPrivileges());

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

            var results = this.purchaseOrderReportFacadeService.GetOrdersBySupplierExport(resource, req.HttpContext.GetPrivileges());

            this.csvStreamWriter.WriteModel(results);

            res.Body = this.stream;
            //how do I get the actual value of the stream?!

            res.ContentType = "text/csv";

            res.Headers.Add("Content-Disposition", $"attachment;filename=ordersBySupplier{resource.From.Substring(0, 10)}_To_{resource.To.Substring(0, 10)}.csv");
            await res.Negotiate(results);
        }



    }
}
