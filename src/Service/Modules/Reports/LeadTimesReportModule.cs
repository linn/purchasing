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

    public class LeadTimesReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/leadtimes", this.GetApp);
            app.MapGet("/purchasing/reports/leadtimes/report", this.GetLeadTimesWithSupplierReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetLeadTimesWithSupplierReport(
            HttpRequest req,
            HttpResponse res,
            int supplier,
            ILeadTimesReportFacadeService reportFacadeService)
        {
            var options = new LeadTimesReportRequestResource()
                              {
                                   Supplier = supplier
                              };

            var results = reportFacadeService.GetLeadTimesWithSupplierReport(options);

            await res.Negotiate(results);
        }
    }
}
