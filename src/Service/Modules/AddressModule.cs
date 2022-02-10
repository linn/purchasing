namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Http;

    public class AddressModule : CarterModule
    {
        private readonly IFacadeResourceFilterService<Address, int, AddressResource, AddressResource, AddressResource>
            addressService;

        public AddressModule(
            IFacadeResourceFilterService<Address, int, AddressResource, AddressResource, AddressResource> addressService)
        {
            this.addressService = addressService;
            this.Post("/purchasing/addresses", this.CreateAddress);
            this.Put("/purchasing/addresses/{id:int}", this.UpdateAddress);
            this.Get("/purchasing/addresses", this.SearchAddresses);
        }

        private async Task CreateAddress(HttpRequest request, HttpResponse response)
        {
            var resource = await request.Bind<AddressResource>();
            var result = this.addressService.Add(
                resource);

            await response.Negotiate(result);
        }

        private async Task UpdateAddress(HttpRequest request, HttpResponse response)
        {
            var id = request.RouteValues.As<int>("id");
            var resource = await request.Bind<AddressResource>();
            var result = this.addressService.Update(
                id,
                resource);

            await response.Negotiate(result);
        }

        private async Task SearchAddresses(HttpRequest req, HttpResponse res)
        {
            var searchTerm = req.Query.As<string>("searchTerm");
            var result = this.addressService.FilterBy(
                new AddressResource
                    {
                        Addressee = searchTerm
                    });

            await res.Negotiate(result);
        }
    }
}
