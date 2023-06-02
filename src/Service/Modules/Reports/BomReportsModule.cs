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
            app.MapGet("/purchasing/boms/reports/diff", this.GetBomDifferenceReport);
            app.MapGet("/purchasing/boms/reports/board-component-summary/report", this.GetBoardComponentSummaryReport);
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

        private async Task GetBomDifferenceReport(
            HttpRequest request,
            HttpResponse response,
            string bom1,
            string bom2,
            IBomReportsFacadeService bomReportsFacadeService)
        {
            if (string.IsNullOrEmpty(bom1))
            {
                await response.Negotiate(new ViewResponse { ViewName = "Index.html" });
            }
            else
            {
                var results = bomReportsFacadeService.GetBomDifferencesReport(bom1, bom2);

                await response.Negotiate(results);
            }
        }

        private async Task GetBoardComponentSummaryReport(
            HttpRequest request,
            HttpResponse response,
            string boardCode,
            string revisionCode,
            IBomReportsFacadeService bomReportsFacadeService)
        {
            var resource = new BoardComponentSummaryReportRequestResource
            {
                BoardCode = boardCode,
                RevisionCode = revisionCode
            };

            var results = bomReportsFacadeService.GetBoardComponentSummaryReport(resource);

            await response.Negotiate(results);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }
    }
}
