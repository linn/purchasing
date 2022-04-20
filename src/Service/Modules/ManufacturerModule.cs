namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ManufacturerModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/manufacturers", this.SearchManufacturers);
        }

        private async Task SearchManufacturers(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource> facadeService)
        {
            var result = facadeService.Search(searchTerm);

            await res.Negotiate(result);
        }
    }
}
