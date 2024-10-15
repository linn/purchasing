namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class LedgerPeriodModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/ledger-periods/{id:int}", this.GetLedgerPeriod);
            app.MapGet("/purchasing/ledger-periods", this.GetLedgerPeriods);
            app.MapGet("/purchasing/ledger-periods/search", this.SearchLedgerPeriods);
        }

        private async Task GetLedgerPeriod(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceService<LedgerPeriod, int, LedgerPeriodResource, LedgerPeriodResource> facadeService)
        {
            var result = facadeService.GetById(id);

            await res.Negotiate(result);
        }

        private async Task GetLedgerPeriods(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<LedgerPeriod, int, LedgerPeriodResource, LedgerPeriodResource> facadeService)
        {
            var result = facadeService.GetAll();

            await res.Negotiate(result);
        }

        private async Task SearchLedgerPeriods(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<LedgerPeriod, int, LedgerPeriodResource, LedgerPeriodResource> ledgerPeriod)
        {
            var result = ledgerPeriod.Search(searchTerm);

            await res.Negotiate(result);
        }
    }
}
