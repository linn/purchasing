namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Http;

    public class ManufacturerModule : CarterModule
    {
        private readonly IFacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource>
            facadeService;

        public ManufacturerModule(IFacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource> facadeService)
        {
            this.facadeService = facadeService;
            this.Get("/purchasing/manufacturers", this.SearchManufacturers);
        }

        private async Task SearchManufacturers(HttpRequest req, HttpResponse res)
        {
            var searchTerm = req.Query.As<string>("searchTerm");

            var result = this.facadeService.Search(searchTerm);

            await res.Negotiate(result);
        }
    }
}
