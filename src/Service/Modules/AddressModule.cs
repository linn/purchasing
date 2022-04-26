namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class AddressModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/purchasing/addresses", this.CreateAddress);
            app.MapPut("/purchasing/addresses/{id:int}", this.UpdateAddress);
            app.MapGet("/purchasing/addresses", this.SearchAddresses);
            app.MapGet("/purchasing/countries", this.SearchCountries);
        }

        private async Task CreateAddress(
            HttpRequest request,
            HttpResponse response,
            AddressResource resource,
            IFacadeResourceFilterService<Address, int, AddressResource, AddressResource, AddressResource> addressService)
        {
            var result = addressService.Add(resource);

            await response.Negotiate(result);
        }

        private async Task UpdateAddress(
            HttpRequest request,
            HttpResponse response,
            int id,
            AddressResource resource,
            IFacadeResourceFilterService<Address, int, AddressResource, AddressResource, AddressResource> addressService)
        {
            var result = addressService.Update(id, resource);

            await response.Negotiate(result);
        }

        private async Task SearchAddresses(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceFilterService<Address, int, AddressResource, AddressResource, AddressResource> addressService)
        {
            var result = addressService.FilterBy(
                new AddressResource
                    {
                        Addressee = searchTerm
                    });

            await res.Negotiate(result);
        }

        private async Task SearchCountries(
            HttpRequest req, 
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<Country, string, CountryResource, CountryResource> countryService)
        {
            var result = countryService.Search(searchTerm);
            await res.Negotiate(result);
        }
    }
}
