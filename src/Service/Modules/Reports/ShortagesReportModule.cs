namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

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
            app.MapGet("/purchasing/reports/shortages/report", this.GetShortagesReport);
            app.MapGet("/purchasing/reports/shortages-planner/report", this.GetShortagesPlannerReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetShortagesReport(
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

            var results = reportFacadeService.GetShortagesReport(options);

            await res.Negotiate(results);
        }

        private async Task GetShortagesPlannerReport(
            HttpRequest req,
            HttpResponse res,
            int planner,
            IShortagesReportFacadeService reportFacadeService)
        {
            var results = reportFacadeService.GetShortagesPlannerReport(planner);

            await res.Negotiate(results);
        }
    }
}
