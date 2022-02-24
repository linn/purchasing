namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class SupplierContactModule : CarterModule
    {
        private readonly IFacadeResourceService<Contact, int, ContactResource, ContactResource> supplierContactService;

        public SupplierContactModule(IFacadeResourceService<Contact, int, ContactResource, ContactResource> supplierContactService)
        {
            this.supplierContactService = supplierContactService;
            this.Post("/purchasing/supplier-groups", this.GetAllSupplierContacts);
            this.Put("/purchasing/supplier-groups/{id:int}", this.GetSupplierContact);
        }

        private async Task GetSupplierContact(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");

            var result = this.supplierContactService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllSupplierContacts(HttpRequest req, HttpResponse res)
        {
            var result = this.supplierContactService.GetAll(req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}