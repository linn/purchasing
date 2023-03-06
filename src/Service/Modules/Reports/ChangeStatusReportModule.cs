using Linn.Purchasing.Resources.RequestResources;

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
            var options = new ChangeStatusReportRequestResource()
            {
                Months = months
            };

            var results = facadeService.GetChangeStatusReport(options);

            await res.Negotiate(results);
        }

        private async Task GetAcceptedChangesReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var options = new ChangeStatusReportRequestResource()
            {
                Months = months
            };

            var results = facadeService.GetAcceptedChangesReport(options);

            await res.Negotiate(results);
        }

        private async Task GetProposedChangesReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var options = new ChangeStatusReportRequestResource()
            {
                Months = months
            };

            var results = facadeService.GetProposedChangesReport(options);

            await res.Negotiate(results);
        }

        private async Task GetTotalOutstandingChangesReport(
            HttpRequest req,
            HttpResponse res,
            int months,
            IChangeStatusReportsFacadeService facadeService)
        {
            var options = new ChangeStatusReportRequestResource()
            {
                Months = months
            };

            var results = facadeService.GetTotalOutstandingChangesReport(options);

            await res.Negotiate(results);
        }

    }
}
