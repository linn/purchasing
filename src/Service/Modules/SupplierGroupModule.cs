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

    public class SupplierGroupModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/supplier-groups", this.GetAllSupplierGroups);
            app.MapGet("/purchasing/supplier-groups/{id:int}", this.GetSupplierGroup);
        }

        private async Task GetSupplierGroup(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource> supplierGroupFacadeService,
            int id)
        {
            var result = supplierGroupFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllSupplierGroups(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource> supplierGroupFacadeService)
        {
            var result = supplierGroupFacadeService.GetAll(req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
