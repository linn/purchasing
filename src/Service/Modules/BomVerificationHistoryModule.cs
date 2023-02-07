namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class BomVerificationHistoryModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/bom-verification/", this.GetApp);
            app.MapGet("/purchasing/bom-verification/{id:int}", this.GetBomVerificationHistoryEntry);
            app.MapPost("/purchasing/bom-verification/", this.CreateBomVerificationHistoryEntry);
            app.MapGet("/purchasing/bom-verification/bom-frequency/{id}", this.GetBomFrequencyEntry);
            app.MapPut("/purchasing/bom-verification/bom-frequency/{id}", this.UpdateBomFrequencyEntry);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetBomVerificationHistoryEntry(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<BomVerificationHistory, int, BomVerificationHistoryResource, BomVerificationHistoryResource> bomVerificationHistoryFacadeService,
            int id)
        {
            var result = bomVerificationHistoryFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateBomVerificationHistoryEntry(
            HttpRequest req,
            HttpResponse res,
            BomVerificationHistoryResource resource,
            IFacadeResourceService<BomVerificationHistory, int, BomVerificationHistoryResource, BomVerificationHistoryResource> bomVerificationHistoryFacadeService)
        {
            var result = bomVerificationHistoryFacadeService.Add(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetBomFrequencyEntry(
            HttpRequest req,
            HttpResponse res,   
            IFacadeResourceService<BomFrequencyWeeks, string, BomFrequencyWeeksResource, BomFrequencyWeeksResource> bomFrequencyFacadeService,
            string id)
        {
            var result = bomFrequencyFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task UpdateBomFrequencyEntry(
        HttpRequest req,
        HttpResponse res,
        string id,
        BomFrequencyWeeksResource resource,
        IFacadeResourceService<BomFrequencyWeeks, string, BomFrequencyWeeksResource, BomFrequencyWeeksResource> bomFrequencyFacadeService)
        {
            var result = bomFrequencyFacadeService.Update(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
