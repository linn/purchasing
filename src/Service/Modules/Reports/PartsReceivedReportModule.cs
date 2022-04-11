using System.Net.Mime;
namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.RequestResources;
    
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class PartsReceivedReportModule : CarterModule
    {
        private readonly ITqmsJobRefService tqmsJobRefService;

        private readonly IPartsReceivedReportFacadeService reportFacadeService;

        public PartsReceivedReportModule(
            ITqmsJobRefService tqmsJobRefService,
            IPartsReceivedReportFacadeService reportFacadeService)
        {
            this.tqmsJobRefService = tqmsJobRefService;
            this.reportFacadeService = reportFacadeService;
            this.Get("/purchasing/tqms-jobrefs", this.GetJobRefs);
            this.Get("/purchasing/reports/parts-received", this.GetReport);
            this.Get("/purchasing/reports/parts-received/export", this.GetExport);
        }

        private async Task GetJobRefs(HttpRequest request, HttpResponse response)
        {
            var result = this.tqmsJobRefService.GetMostRecentJobRefs(50);
            await response.Negotiate(result);
        }

        private async Task GetReport(HttpRequest req, HttpResponse res)
        {
            var options = new PartsReceivedReportRequestResource
                              {
                                  Supplier = req.Query.As<int?>("supplier"),
                                  FromDate = req.Query.As<string>("fromDate"),
                                  ToDate = req.Query.As<string>("toDate"),
                                  Jobref = req.Query.As<string>("jobref"),
                                  OrderBy = req.Query.As<string>("orderBy"),
                                  IncludeNegativeValues = req.Query.As<bool>("includeNegativeValues")
                              };
            if (string.IsNullOrEmpty(options.ToDate))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
                return;
            }
            
            var results = this.reportFacadeService.GetReport(options);

            await res.Negotiate(results);
        }

        private async Task GetExport(HttpRequest req, HttpResponse res)
        {
            var options = new PartsReceivedReportRequestResource
                {
                    Supplier = req.Query.As<int?>("supplier"),
                    FromDate = req.Query.As<string>("fromDate"),
                    ToDate = req.Query.As<string>("toDate"),
                    Jobref = req.Query.As<string>("jobref"),
                    OrderBy = req.Query.As<string>("orderBy"),
                    IncludeNegativeValues = req.Query.As<bool>("includeNegativeValues")
                };
            
            var csv = this.reportFacadeService.GetReportCsv(options);

            await res.FromCsv(csv, "parts_received.csv");
        }
    }
}
