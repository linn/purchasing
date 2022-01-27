namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class SupplierModule : CarterModule
    {
        private readonly
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>
            partSupplierFacadeService;

        private readonly
            IFacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey, PreferredSupplierChangeResource,
                PreferredSupplierChangeKey> preferredSupplierChangeService;

        private readonly IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService;

        private readonly IFacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource, PriceChangeReasonResource> 
            priceChangeReasonService;

        private readonly IPartService partFacadeService;

        public SupplierModule(
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> partSupplierFacadeService,
            IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> supplierFacadeService,
            IFacadeResourceService<PreferredSupplierChange, PreferredSupplierChangeKey, PreferredSupplierChangeResource, PreferredSupplierChangeKey> preferredSupplierChangeService,
            IPartService partFacadeService,
            IFacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource, PriceChangeReasonResource> priceChangeReasonService)
        {
            this.supplierFacadeService = supplierFacadeService;
            this.partSupplierFacadeService = partSupplierFacadeService;
            this.partFacadeService = partFacadeService;
            this.preferredSupplierChangeService = preferredSupplierChangeService;
            this.priceChangeReasonService = priceChangeReasonService;
            this.Get("/purchasing/part-suppliers/record", this.GetById);
            this.Put("/purchasing/part-suppliers/record", this.UpdatePartSupplier);
            this.Get("/purchasing/part-suppliers", this.SearchPartSuppliers);
            this.Post("/purchasing/part-suppliers/record", this.CreatePartSupplier);

            this.Get("/purchasing/part-suppliers/application-state", this.GetPartSuppliersState);
            this.Get("/purchasing/suppliers", this.SearchSuppliers);
            this.Post("/purchasing/preferred-supplier-changes", this.CreatePreferredSupplierChange);
            this.Get("/purchasing/price-change-reasons", this.GetPriceChangeReasons);
            this.Get("/purchasing/part-suppliers/part-price-conversions", this.GetPartPriceConversions);
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
            var resource = await req.Bind<PartSupplierResource>();

            var key = new PartSupplierKey { PartNumber = resource.PartNumber, SupplierId = resource.SupplierId };

            var result = this.partSupplierFacadeService.Update(
                key,
                resource,
                req.HttpContext.GetPrivileges());

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

        private async Task GetPartSuppliersState(HttpRequest req, HttpResponse res)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = this.partSupplierFacadeService.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task CreatePartSupplier(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PartSupplierResource>();
            var result = this.partSupplierFacadeService.Add(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreatePreferredSupplierChange(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PreferredSupplierChangeResource>();

            var privileges = req.HttpContext.GetPrivileges();
            var result = this.preferredSupplierChangeService.Add(
                resource,
                privileges);

            await res.Negotiate(result);
        }

        private async Task GetPriceChangeReasons(HttpRequest req, HttpResponse res)
        {
            var result = this.priceChangeReasonService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetPartPriceConversions(HttpRequest req, HttpResponse res)
        {
            var partNumber = req.Query.As<string>("partNumber");
            var newCurrency = req.Query.As<string>("newCurrency");
            var newPrice = req.Query.As<decimal>("newPrice");
            var ledger = req.Query.As<string>("ledger");
            var round = req.Query.As<string>("round");


            var result = this.partFacadeService.GetPrices(
                partNumber, 
                newCurrency, 
                newPrice,
                ledger,
                round);

            await res.Negotiate(result);
        }
    }
}
