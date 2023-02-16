namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class BomHistoryReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/bom-history/options", this.GetApp);
            app.MapGet("/purchasing/reports/bom-history", this.GetReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetReport(
            HttpRequest req,
            HttpResponse res,
            string bomName,
            string from,
            string to,
            bool includeSubAssemblies,
            IBomHistoryReportFacadeService service)
        {
            var results = service.GetReport(bomName, from, to, includeSubAssemblies);

            await res.Negotiate(results);
        }
    }
}
