namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    using Microsoft.AspNetCore.Http;

    public class PartSupplierModule : CarterModule
    {
        private readonly
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>
            facadeService;

        private readonly IPartService partFacadeService;

        public PartSupplierModule(
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> facadeService,
            IPartService partFacadeService)
        {
            this.facadeService = facadeService;
            this.partFacadeService = partFacadeService;
            this.Get("/purchasing/part-supplier", this.GetById);
            this.Get("/purchasing/part-suppliers", this.Search);
        }

        private async Task GetById(HttpRequest req, HttpResponse res)
        {
            var partId = req.Query.As<int>("partId");
            var supplierId = req.Query.As<int>("supplierId");

            var partNumber = this.partFacadeService.GetPartNumberFromId(partId);

            var result = this.facadeService.GetById(
                new PartSupplierKey 
                    {
                        PartNumber = partNumber,
                        SupplierId = supplierId
                });

            await res.Negotiate(result);
        }

        private async Task Search(HttpRequest req, HttpResponse res)
        {
            var partNumberSearch = req.Query.As<string>("partNumber");
            var supplierNameSearch = req.Query.As<string>("supplierName");
            var result = this.facadeService.FilterBy(new PartSupplierSearchResource
                                                         {
                                                             PartNumberSearchTerm = partNumberSearch,
                                                             SupplierNameSearchTerm = supplierNameSearch
                                                         });
            await res.Negotiate(result);
        }
    }
}
