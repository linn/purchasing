namespace Linn.Purchasing.Service.Modules.Reports
{
    using System;
    using System.Net.Mime;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade.Carter.Extensions;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class SpendsReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/spend-by-supplier", this.GetApp);
            app.MapGet("/purchasing/reports/spend-by-part", this.GetApp);
            app.MapGet("/purchasing/reports/spend-by-supplier-by-date-range", this.GetApp);
            app.MapGet("/purchasing/reports/spend-by-supplier/report", this.GetSpendBySupplierReport);
            app.MapGet("/purchasing/reports/spend-by-supplier/export", this.GetSpendBySupplierExport);
            app.MapGet("/purchasing/reports/spend-by-supplier-by-date-range/report", this.GetSpendBySupplierByDateRangeReport);
            app.MapGet("/purchasing/reports/spend-by-supplier-by-date-range/export", this.GetSpendBySupplierByDateRangeExport);
            app.MapGet("/purchasing/reports/spend-by-part-by-date/report", this.GetSpendByPartByDateReport);
            app.MapGet("/purchasing/reports/spend-by-part/report", this.GetSpendByPartReport);
            app.MapGet("/purchasing/reports/spend-by-part/export", this.GetSpendByPartExport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetSpendByPartExport(
            HttpRequest req,
            HttpResponse res,
            ISpendsReportFacadeService spendsReportFacadeService,
            int id)
        {
            var csv = spendsReportFacadeService.GetSpendByPartExport(id);
            
            await res.FromCsv(csv, $"spendByPart_{id}_{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
        }

        private async Task GetSpendByPartReport(
            HttpRequest req,
            HttpResponse res,
            int id,
            ISpendsReportFacadeService spendsReportFacadeService)
        {
            var results = spendsReportFacadeService.GetSpendByPartReport(id);

            await res.Negotiate(results);
        }

        private async Task GetSpendByPartByDateReport(
            HttpRequest req,
            HttpResponse res,
            int id,
            string fromDate,
            string toDate,
            ISpendsReportFacadeService spendsReportFacadeService)
        {
            var options = new SpendBySupplierByDateRangeReportRequestResource
                              {
                                  FromDate = fromDate,
                                  ToDate = toDate,
                                  SupplierId = id
                              };
            var results = spendsReportFacadeService.GetSpendByPartByDateReport(options);

            await res.Negotiate(results);
        }

        private async Task GetSpendBySupplierExport(
            HttpRequest req,
            HttpResponse res,
            ISpendsReportFacadeService spendsReportFacadeService,
            string vm)
        {
            var csv = spendsReportFacadeService.GetSpendBySupplierExport(vm ?? string.Empty);

            var contentDisposition = new ContentDisposition
                                         {
                                             FileName = $"spendBySuppliers{DateTime.Now.ToString("dd-MM-yyyy")}.csv"
                                         };

            await res.FromCsv(csv, $"spendBySuppliers{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
        }

        private async Task GetSpendBySupplierReport(
            HttpRequest req,
            HttpResponse res,
            string vm,
            ISpendsReportFacadeService spendsReportFacadeService)
        {
            var results = spendsReportFacadeService.GetSpendBySupplierReport(vm ?? string.Empty);

            await res.Negotiate(results);
        }

        private async Task GetSpendBySupplierByDateRangeExport(
            HttpRequest req,
            HttpResponse res,
            ISpendsReportFacadeService spendsReportFacadeService,
            string fromDate,
            string toDate,
            string vm,
            int? supplierId)
        {
            var options = new SpendBySupplierByDateRangeReportRequestResource
            {
                VendorManager = vm ?? string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
                SupplierId = supplierId
            };

            var csv = spendsReportFacadeService.GetSpendBySupplierByDateRangeReportExport(options);

            await res.FromCsv(csv, $"spendBySuppliersByDateRange{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
        }

        private async Task GetSpendBySupplierByDateRangeReport(
            HttpRequest req,
            HttpResponse res,
            string fromDate,
            string toDate,
            string vm,
            ISpendsReportFacadeService spendsReportFacadeService)
        {
            var options = new SpendBySupplierByDateRangeReportRequestResource
            {
                VendorManager = vm ?? string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
            };

            var results = spendsReportFacadeService.GetSpendBySupplierByDateRangeReport(options);

            await res.Negotiate(results);
        }
    }
}
