namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class SupplierGroupModule : CarterModule
    {
        private readonly IFacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource> supplierGroupFacadeService;

        public SupplierGroupModule(IFacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource> supplierGroupFacadeService)
        {
            this.supplierGroupFacadeService = supplierGroupFacadeService;
            this.Get("/purchasing/supplier-groups", this.GetAllSupplierGroups);
            this.Get("/purchasing/supplier-groups/{id:int}", this.GetSupplierGroup);
        }

        private async Task GetSupplierGroup(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");

            var result = this.supplierGroupFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllSupplierGroups(HttpRequest req, HttpResponse res)
        {
            var result = this.supplierGroupFacadeService.GetAll(req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
