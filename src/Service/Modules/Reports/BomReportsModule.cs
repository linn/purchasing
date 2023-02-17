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

    public class BomReportsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/board-difference", this.GetApp);
            app.MapGet("/purchasing/reports/board-difference/report", this.GetBoardDifferenceReport);
        }

        private async Task GetBoardDifferenceReport(
            HttpRequest request,
            HttpResponse response,
            string boardCode1,
            string boardCode2,
            string revisionCode1,
            string revisionCode2,
            IBomReportsFacadeService bomReportsFacadeService)
        {
            var resource = new BomDifferenceReportRequestResource
                               {
                                   BoardCode1 = boardCode1,
                                   RevisionCode1 = revisionCode1,
                                   BoardCode2 = boardCode2,
                                   RevisionCode2 = revisionCode2
                               };

            var results = bomReportsFacadeService.GetBoardDifferenceReport(resource);

            await response.Negotiate(results);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }
    }
}
