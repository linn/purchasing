namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade.Carter.Extensions;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class WhatsDueInReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/whats-due-in", this.GetReport);
        }

        private async Task GetReport(
            HttpRequest req,
            HttpResponse res,
            IWhatsDueInReportFacadeService reportFacadeService,
            string fromDate,
            string toDate,
            string orderBy,
            string vendorManager,
            int? supplier)
        {
            if (string.IsNullOrEmpty(fromDate)) 
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
                return;
            }

            var results = reportFacadeService.GetReport(fromDate, toDate, orderBy, vendorManager, supplier);

            await res.Negotiate(results);
        }
    }
}
