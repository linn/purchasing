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

    public class ShortagesReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/shortages", this.GetApp);
            app.MapGet("/purchasing/reports/shortages/report", this.GetReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetReport(
            HttpRequest req,
            HttpResponse res,
            string purchaseLevel, 
            string vendorManager,
            IShortagesReportFacadeService reportFacadeService)
        {
            var options = new ShortagesReportRequestResource
                              {
                                  PurchaseLevel = purchaseLevel,
                                  VendorManager = vendorManager
                              };

            var results = reportFacadeService.GetReport(options);

            await res.Negotiate(results);
        }
    }
}
