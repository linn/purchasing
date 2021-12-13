namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Http;

    public class PurchaseOrderModule : CarterModule
    {
        private readonly IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService;

        public PurchaseOrderModule(
            IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService)
        {
            this.currencyService = currencyService;

            this.Get("/purchasing/purchase-orders/currencies", this.GetCurrencies);
        }

        private async Task GetCurrencies(HttpRequest req, HttpResponse res)
        {
            var result = this.currencyService.GetAll();

            await res.Negotiate(result);
        }
    }
}
