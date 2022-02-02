namespace Linn.Purchasing.Service.Modules.Reports
{
    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class SpendsReportModule : CarterModule
    {
        private readonly ISpendsReportFacadeService spendsReportFacadeService;

        public SpendsReportModule(ISpendsReportFacadeService spendsReportFacadeService)
        {
            this.spendsReportFacadeService = spendsReportFacadeService;
            this.Get("/purchasing/reports/spend-by-supplier", this.GetApp);
            this.Get("/purchasing/reports/spend-by-supplier/report", this.GetSpendBySupplierReport);
            this.Get("/purchasing/reports/spend-by-supplier/export", this.GetSpendBySupplierExport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetSpendBySupplierExport(HttpRequest req, HttpResponse res)
        {
            //var resource = new OrdersBySupplierSearchResource();
            //resource.SupplierId = req.Query.As<int>("Id");
          

            using Stream stream = this.spendsReportFacadeService.GetSpendBySupplierExport(req.HttpContext.GetPrivileges());

            var contentDisposition = new ContentDisposition
                                         {
                                             FileName =
                                                 $"spendBySuppliers{DateTime.Now.ToString("dd-MM-yyyy")}.csv"
                                         };

            stream.Position = 0;
            await res.FromStream(stream, "text/csv", contentDisposition);
        }

        private async Task GetSpendBySupplierReport(HttpRequest req, HttpResponse res)
        {
            //var resource = new OrdersBySupplierSearchResource();
            //resource.SupplierId = req.Query.As<int>("Id");

            var results = this.spendsReportFacadeService.GetSpendBySupplierReport(req.HttpContext.GetPrivileges());

            await res.Negotiate(results);
        }
    }
}
