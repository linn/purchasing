namespace Linn.Purchasing.Service.Modules.Reports
{
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

    public class PartsReceivedReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/tqms-jobrefs", this.GetJobRefs);
            app.MapGet("/purchasing/reports/parts-received", this.GetReport);
            app.MapGet("/purchasing/reports/parts-received/export", this.GetExport);
        }

        private async Task GetJobRefs(
            HttpRequest request,
            HttpResponse response,
            ITqmsJobRefService tqmsJobRefService)
        {
            var result = tqmsJobRefService.GetMostRecentJobRefs(50);
            await response.Negotiate(result);
        }

        private async Task GetReport(
            HttpRequest req,
            HttpResponse res,
            int? supplier,
            string fromDate,
            string toDate,
            string jobref,
            string orderBy,
            bool includeNegativeValues,
            IPartsReceivedReportFacadeService reportFacadeService)
        {
            var options = new PartsReceivedReportRequestResource
                              {
                                  Supplier = supplier,
                                  FromDate = fromDate,
                                  ToDate = toDate, 
                                  Jobref = jobref, 
                                  OrderBy = orderBy, 
                                  IncludeNegativeValues = includeNegativeValues
                              };
            if (string.IsNullOrEmpty(options.ToDate))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
                return;
            }
            
            var results = reportFacadeService.GetReport(options);

            await res.Negotiate(results);
        }

        private async Task GetExport(
            HttpRequest req,
            HttpResponse res,
            int? supplier,
            string fromDate,
            string toDate,
            string jobref,
            string orderBy,
            bool includeNegativeValues,
            IPartsReceivedReportFacadeService reportFacadeService)
        {
            var options = new PartsReceivedReportRequestResource
                {
                    Supplier = supplier, 
                    FromDate = fromDate, 
                    ToDate = toDate, 
                    Jobref = jobref, 
                    OrderBy = orderBy, 
                    IncludeNegativeValues = includeNegativeValues
                };
            
            var csv = reportFacadeService.GetReportCsv(options);

            await res.FromCsv(csv, "parts_received.csv");
        }
    }
}
