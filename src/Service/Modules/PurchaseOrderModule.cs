namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Http;

    public class PurchaseOrderModule : CarterModule
    {
        private readonly IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService;

        private readonly IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> orderMethodService;

        private readonly IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> 
            deliveryAddressService;


        public PurchaseOrderModule(
            IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService,
            IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> orderMethodService,
            IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> deliveryAddressService)
        {
            this.currencyService = currencyService;
            this.orderMethodService = orderMethodService;
            this.deliveryAddressService = deliveryAddressService;

            this.Get("/purchasing/purchase-orders/currencies", this.GetCurrencies);
            this.Get("/purchasing/purchase-orders/methods", this.GetOrderMethods);
            this.Get("/purchasing/purchase-orders/delivery-addresses", this.GetDeliveryAddresses);
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
    }
}
