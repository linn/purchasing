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
            app.MapGet("/purchasing/bom-verification/{id:int}", this.GetBomVerificationHistoryEntry);
            app.MapPost("/purchasing/bom-verification/", this.CreateBomVerificationHistoryEntry);
            app.MapGet("/purchasing/bom-verification/", this.SearchBomVerificationHistoryEntries);
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

        private async Task SearchBomVerificationHistoryEntries(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<BomVerificationHistory, int, BomVerificationHistoryResource, BomVerificationHistoryResource> bomVerificationHistoryFacadeService)
        {
            var result = bomVerificationHistoryFacadeService.Search(searchTerm);

            await res.Negotiate(result);
        }
    }
}
