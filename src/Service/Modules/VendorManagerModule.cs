namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class VendorManagerModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/vendor-managers/{id}", this.GetVendorManager);
            app.MapGet("/purchasing/vendor-managers", this.GetAllVendorManagers);
        }

        private async Task GetVendorManager(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource> facadeService,
            string id)
        {
            var result = facadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllVendorManagers(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource> facadeService)
        {
            var result = facadeService.GetAll();

            await res.Negotiate(result);
        }
    }
}
