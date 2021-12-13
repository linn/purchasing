namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class SupplierModule : CarterModule
    {
        private readonly
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>
            partSupplierFacadeService;

        private readonly IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService;

        private readonly IPartService partFacadeService;

        public SupplierModule(
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> partSupplierFacadeService,
            IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService,
            IPartService partFacadeService)
        {
            this.supplierFacadeService = supplierFacadeService;
            this.partSupplierFacadeService = partSupplierFacadeService;
            this.partFacadeService = partFacadeService;
            this.Get("/purchasing/part-suppliers/record", this.GetById);
            this.Put("/purchasing/part-suppliers/record", this.UpdatePartSupplier);
            this.Get("/purchasing/part-suppliers", this.SearchPartSuppliers);
            this.Post("/purchasing/part-suppliers", this.CreatePartSupplier);

            this.Get("/purchasing/part-suppliers/application-state", this.GetState);
            this.Get("/purchasing/suppliers", this.SearchSuppliers);
        }

        private async Task GetById(HttpRequest req, HttpResponse res)
        {
            var partId = req.Query.As<int>("partId");
            var supplierId = req.Query.As<int>("supplierId");

            var partNumber = this.partFacadeService.GetPartNumberFromId(partId);

            var result = this.partSupplierFacadeService.GetById(
                new PartSupplierKey 
                    {
                        PartNumber = partNumber,
                        SupplierId = supplierId
                },
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task UpdatePartSupplier(HttpRequest req, HttpResponse res)
        {
            var partId = req.Query.As<int>("partId");
            var supplierId = req.Query.As<int>("supplierId");

            var partNumber = this.partFacadeService.GetPartNumberFromId(partId);

            var key = new PartSupplierKey { PartNumber = partNumber, SupplierId = supplierId };

            var resource = await req.Bind<PartSupplierResource>();
            resource.Privileges = req.HttpContext.GetPrivileges();
            var result = this.partSupplierFacadeService.Update(
                key,
                resource,
                resource.Privileges);

            await res.Negotiate(result);
        }

        private async Task SearchPartSuppliers(HttpRequest req, HttpResponse res)
        {
            var partNumberSearch = req.Query.As<string>("partNumber");
            var supplierNameSearch = req.Query.As<string>("supplierName");
            var result = this.partSupplierFacadeService.FilterBy(
                new PartSupplierSearchResource
                    {
                        PartNumberSearchTerm = partNumberSearch,
                        SupplierNameSearchTerm = supplierNameSearch
                    },
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task SearchSuppliers(HttpRequest req, HttpResponse res)
        {
            var searchTerm = req.Query.As<string>("searchTerm");

            var result = this.supplierFacadeService.Search(searchTerm);

            await res.Negotiate(result);
        }

        private async Task GetState(HttpRequest req, HttpResponse res)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = this.partSupplierFacadeService.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task CreatePartSupplier(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PartSupplierResource>();
            resource.Privileges = req.HttpContext.GetPrivileges();
            var result = this.partSupplierFacadeService.Add(
                resource,
                resource.Privileges);

            await res.Negotiate(result);
        }
    }
}
