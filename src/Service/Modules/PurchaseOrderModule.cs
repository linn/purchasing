namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class PurchaseOrderModule : CarterModule
    {
        private readonly IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService;

        private readonly
            IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> deliveryAddressService;

        private readonly IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> orderMethodService;

        private readonly IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource> packagingGroupService;

        private readonly IFacadeResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource> purchaseOrderFacadeService;

        private readonly IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService;

        private readonly
            IFacadeResourceService<PurchaseOrderReqState, string, PurchaseOrderReqStateResource, PurchaseOrderReqStateResource> purchaseOrderReqStateService;

        private readonly IFacadeResourceService<Tariff, int, TariffResource, TariffResource> tariffService;

        private readonly IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource> unitsOfMeasureService;

        public PurchaseOrderModule(
            IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService,
            IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> orderMethodService,
            IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> deliveryAddressService,
            IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource> unitsOfMeasureService,
            IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource> packagingGroupService,
            IFacadeResourceService<Tariff, int, TariffResource, TariffResource> tariffService,
            IFacadeResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource> purchaseOrderFacadeService,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService,
            IFacadeResourceService<PurchaseOrderReqState, string, PurchaseOrderReqStateResource, PurchaseOrderReqStateResource> purchaseOrderReqStateService)
        {
            this.currencyService = currencyService;
            this.orderMethodService = orderMethodService;
            this.deliveryAddressService = deliveryAddressService;
            this.unitsOfMeasureService = unitsOfMeasureService;
            this.packagingGroupService = packagingGroupService;
            this.tariffService = tariffService;
            this.purchaseOrderFacadeService = purchaseOrderFacadeService;
            this.purchaseOrderReqFacadeService = purchaseOrderReqFacadeService;
            this.purchaseOrderReqStateService = purchaseOrderReqStateService;

            this.Get("/purchasing/purchase-orders/{orderNumber:int}/allow-over-book/", this.GetApp);
            this.Get("/purchasing/purchase-orders/allow-over-book", this.GetApp);
            this.Get("/purchasing/purchase-orders/currencies", this.GetCurrencies);
            this.Get("/purchasing/purchase-orders/methods", this.GetOrderMethods);
            this.Get("/purchasing/purchase-orders/delivery-addresses", this.GetDeliveryAddresses);
            this.Get("/purchasing/purchase-orders/units-of-measure", this.GetUnitsOfMeasure);
            this.Get("/purchasing/purchase-orders/packaging-groups", this.GetPackagingGroups);
            this.Get("/purchasing/purchase-orders/tariffs", this.SearchTariffs);
            this.Get("/purchasing/purchase-orders", this.SearchPurchaseOrders);
            this.Get("/purchasing/purchase-orders/{orderNumber:int}", this.GetPurchaseOrder);
            this.Put("/purchasing/purchase-orders/{orderNumber:int}", this.UpdatePurchaseOrder);

            this.Get("/purchasing/purchase-orders/reqs", this.SearchReqs);
            this.Get("/purchasing/purchase-orders/reqs/states", this.GetReqStates);
            this.Get("/purchasing/purchase-orders/reqs/print", this.GetApp);
            this.Get("/purchasing/purchase-orders/reqs/create", this.GetApp);

            this.Get("/purchasing/purchase-orders/reqs/application-state", this.GetReqApplicationState);
            this.Get("/purchasing/purchase-orders/reqs/{id:int}", this.GetReq);
            this.Put("/purchasing/purchase-orders/reqs/{id:int}", this.UpdateReq);
            this.Post("/purchasing/purchase-orders/reqs/{id:int}/cancel", this.CancelReq);
            this.Post("/purchasing/purchase-orders/reqs/{id:int}/authorise", this.AuthoriseReq);
            this.Post("/purchasing/purchase-orders/reqs", this.CreateReq);
        }

        private async Task AuthoriseReq(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");
            var result = this.purchaseOrderReqFacadeService.Authorise(id, req.HttpContext.GetPrivileges(), req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task CancelReq(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");
            var result = this.purchaseOrderReqFacadeService.DeleteOrObsolete(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateReq(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PurchaseOrderReqResource>();
            var result = this.purchaseOrderReqFacadeService.Add(resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetCurrencies(HttpRequest req, HttpResponse res)
        {
            var result = this.currencyService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetDeliveryAddresses(HttpRequest req, HttpResponse res)
        {
            var result = this.deliveryAddressService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetOrderMethods(HttpRequest req, HttpResponse res)
        {
            var result = this.orderMethodService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetPackagingGroups(HttpRequest req, HttpResponse res)
        {
            var result = this.packagingGroupService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetPurchaseOrder(HttpRequest req, HttpResponse res)
        {
            var orderNumber = req.RouteValues.As<int>("orderNumber");
            var result = this.purchaseOrderFacadeService.GetById(orderNumber, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetReq(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");

            var result = this.purchaseOrderReqFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetReqApplicationState(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(
                this.purchaseOrderReqFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task GetReqStates(HttpRequest req, HttpResponse res)
        {
            var result = this.purchaseOrderReqStateService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetUnitsOfMeasure(HttpRequest req, HttpResponse res)
        {
            var result = this.unitsOfMeasureService.GetAll();

            await res.Negotiate(result);
        }

        private async Task SearchPurchaseOrders(HttpRequest req, HttpResponse res)
        {
            var orderNumberSearch = req.Query.As<string>("searchTerm");
            var result = this.purchaseOrderFacadeService.Search(orderNumberSearch, req.HttpContext.GetPrivileges());
            await res.Negotiate(result);
        }

        private async Task SearchReqs(HttpRequest req, HttpResponse res)
        {
            var reqNumberSearch = req.Query.As<string>("reqNumber");
            var partSearch = req.Query.As<string>("part");
            var supplierSearch = req.Query.As<string>("supplier");

            var result = this.purchaseOrderReqFacadeService.FilterBy(
                new PurchaseOrderReqSearchResource
                    {
                        ReqNumber = reqNumberSearch, Part = partSearch, Supplier = supplierSearch
                    },
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task SearchTariffs(HttpRequest req, HttpResponse res)
        {
            var searchTerm = req.Query.As<string>("searchTerm");
            var result = this.tariffService.Search(searchTerm);

            await res.Negotiate(result);
        }

        private async Task UpdatePurchaseOrder(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PurchaseOrderResource>();
            resource.Privileges = req.HttpContext.GetPrivileges();

            var result = this.purchaseOrderFacadeService.Update(resource.OrderNumber, resource, resource.Privileges, res.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task UpdateReq(HttpRequest req, HttpResponse res)
        {
            var id = req.RouteValues.As<int>("id");
            var resource = await req.Bind<PurchaseOrderReqResource>();
            var result = this.purchaseOrderReqFacadeService.Update(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
