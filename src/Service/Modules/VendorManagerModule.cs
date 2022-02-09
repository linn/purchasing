namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Http;

    public class VendorManagerModule : CarterModule
    {
        private readonly IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource> facadeService;

        public VendorManagerModule(
            IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource> facadeService)
        {
            this.facadeService = facadeService;
            this.Get("/purchasing/vendor-managers", this.GetAllVendorManagers);
        }

        private async Task GetAllVendorManagers(HttpRequest req, HttpResponse res)
        {
            var result = this.facadeService.GetAll();

            await res.Negotiate(result);
        }
    }
}
