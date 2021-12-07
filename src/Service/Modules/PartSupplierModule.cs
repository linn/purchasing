namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Http;

    public class PartSupplierModule : CarterModule
    {
        private readonly
            IFacadeResourceService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource>
            facadeService;

        public PartSupplierModule(
            IFacadeResourceService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource> facadeService)
        {
            this.facadeService = facadeService;
            this.Get("/purchasing/part-supplier", this.GetById);
            this.Get("/purchasing/part-suppliers", this.GetAll);
        }

        private async Task GetById(HttpRequest req, HttpResponse res)
        {
            var result = this.facadeService.GetById(
                new PartSupplierKey 
                    {
                        PartNumber = "RES 202",
                        SupplierId = 41193
                    });

            await res.Negotiate(result);
        }

        private async Task GetAll(HttpRequest req, HttpResponse res)
        {
            var result = this.facadeService.GetAll();
            await res.Negotiate(result);
        }
    }
}