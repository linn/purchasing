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

    public class ChangeStatusReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/change-status", this.GetApp);
            app.MapGet("/purchasing/reports/change-status/report", this.GetChangeStatusReport);
            app.MapGet("/purchasing/reports/accepted-changes/report", this.GetAcceptedChangesReport);
            app.MapGet("/purchasing/reports/proposed-changes/report", this.GetProposedChangesReport);
            app.MapGet("/purchasing/reports/outstanding-changes/report", this.GetTotalOutstandingChangesReport);
            app.MapGet("/purchasing/reports/current-phase-in-weeks/report", this.GetCurrentPhaseInWeeksReport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetChangeStatusReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var results = facadeService.GetChangeStatusReport(months);

            await res.Negotiate(results);
        }

        private async Task GetAcceptedChangesReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var results = facadeService.GetAcceptedChangesReport(months);

            await res.Negotiate(results);
        }

        private async Task GetProposedChangesReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var results = facadeService.GetProposedChangesReport(months);

            await res.Negotiate(results);
        }

        private async Task GetTotalOutstandingChangesReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var results = facadeService.GetTotalOutstandingChangesReport(months);

            await res.Negotiate(results);
        }

        private async Task GetCurrentPhaseInWeeksReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var results = facadeService.GetCurrentPhaseInWeeksReport(months);

            await res.Negotiate(results);
        }
    }
}
