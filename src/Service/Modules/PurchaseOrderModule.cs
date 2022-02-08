namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class PurchaseOrderModule : CarterModule
    {
        private readonly IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService;

        private readonly IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> orderMethodService;

        private readonly IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> 
            deliveryAddressService;

        private readonly IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource>
            unitsOfMeasureService;

        private readonly IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource>
            packagingGroupService;

        private readonly IFacadeResourceService<Tariff, int, TariffResource, TariffResource> tariffService;

        private readonly
            IFacadeResourceFilterService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource>
            purchaseOrderFacadeService;

        public PurchaseOrderModule(
            IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService,
            IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> orderMethodService,
            IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> deliveryAddressService,
            IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource> unitsOfMeasureService,
            IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource> packagingGroupService,
            IFacadeResourceService<Tariff, int, TariffResource, TariffResource> tariffService,
            IFacadeResourceFilterService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource, PurchaseOrderSearchResource> purchaseOrderFacadeService)
        {
            this.currencyService = currencyService;
            this.orderMethodService = orderMethodService;
            this.deliveryAddressService = deliveryAddressService;
            this.unitsOfMeasureService = unitsOfMeasureService;
            this.packagingGroupService = packagingGroupService;
            this.tariffService = tariffService;
            this.purchaseOrderFacadeService = purchaseOrderFacadeService;
            this.Get("/purchasing/purchase-orders/currencies", this.GetCurrencies);
            this.Get("/purchasing/purchase-orders/methods", this.GetOrderMethods);
            this.Get("/purchasing/purchase-orders/delivery-addresses", this.GetDeliveryAddresses);
            this.Get("/purchasing/purchase-orders/units-of-measure", this.GetUnitsOfMeasure);
            this.Get("/purchasing/purchase-orders/packaging-groups", this.GetPackagingGroups);
            this.Get("/purchasing/purchase-orders/tariffs", this.SearchTariffs);
            this.Get("/purchasing/purchase-orders/overbook", this.SearchPurchaseOrders);
            this.Put("/purchasing/purchase-orders/overbook", this.UpdatePurchaseOrder);
        }

        private async Task GetCurrencies(HttpRequest req, HttpResponse res)
        {
            var result = this.currencyService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetOrderMethods(HttpRequest req, HttpResponse res)
        {
            var result = this.orderMethodService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetDeliveryAddresses(HttpRequest req, HttpResponse res)
        {
            var result = this.deliveryAddressService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetUnitsOfMeasure(HttpRequest req, HttpResponse res)
        {
            var result = this.unitsOfMeasureService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetPackagingGroups(HttpRequest req, HttpResponse res)
        {
            var result = this.packagingGroupService.GetAll();

            await res.Negotiate(result);
        }

        private async Task SearchTariffs(HttpRequest req, HttpResponse res)
        {
            var searchTerm = req.Query.As<string>("searchTerm");
            var result = this.tariffService.Search(searchTerm);

            await res.Negotiate(result);
        }

        private async Task SearchPurchaseOrders(HttpRequest req, HttpResponse res)
        {
            var orderNumberSearch = req.Query.As<int>("orderNumber");
            var result = this.purchaseOrderFacadeService.FilterBy(
                new PurchaseOrderSearchResource
                    {
                        OrderNumberSearchTerm = orderNumberSearch
                    },
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task UpdatePurchaseOrder(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PurchaseOrderResource>();
            resource.Privileges = req.HttpContext.GetPrivileges();
           
            var result = this.purchaseOrderFacadeService.Update(
                resource.OrderNumber,
                resource,
                resource.Privileges);

            await res.Negotiate(result);
        }
    }
}
