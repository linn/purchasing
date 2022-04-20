﻿namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PurchaseOrderModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/purchase-orders/application-state", this.GetApplicationState);
            app.MapGet("/purchasing/purchase-orders/{orderNumber:int}/allow-over-book/", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/allow-over-book", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/currencies", this.GetCurrencies);
            app.MapGet("/purchasing/purchase-orders/methods", this.GetOrderMethods);
            app.MapGet("/purchasing/purchase-orders/delivery-addresses", this.GetDeliveryAddresses);
            app.MapGet("/purchasing/purchase-orders/units-of-measure", this.GetUnitsOfMeasure);
            app.MapGet("/purchasing/purchase-orders/packaging-groups", this.GetPackagingGroups);
            app.MapGet("/purchasing/purchase-orders/tariffs", this.SearchTariffs);
            app.MapGet("/purchasing/purchase-orders", this.SearchPurchaseOrders);
            app.MapGet("/purchasing/purchase-orders/{orderNumber:int}", this.GetPurchaseOrder);
            app.MapPut("/purchasing/purchase-orders/{orderNumber:int}", this.UpdatePurchaseOrder);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetCurrencies(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> currencyService)
        {
            var result = currencyService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetDeliveryAddresses(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<LinnDeliveryAddress, int, LinnDeliveryAddressResource, LinnDeliveryAddressResource> deliveryAddressService)
        {
            var result = deliveryAddressService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetOrderMethods(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource> orderMethodService)
        {
            var result = orderMethodService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetPackagingGroups(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<PackagingGroup, int, PackagingGroupResource, PackagingGroupResource> packagingGroupService)
        {
            var result = packagingGroupService.GetAll();

            await res.Negotiate(result);
        }

        private async Task GetPurchaseOrder(
            HttpRequest req,
            HttpResponse res,
            int orderNumber,
            IFacadeResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource> purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.GetById(orderNumber, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetApplicationState(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource> purchaseOrderFacadeService)
        {
            await res.Negotiate(purchaseOrderFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task GetUnitsOfMeasure(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource> unitsOfMeasureService)
        {
            var result = unitsOfMeasureService.GetAll();

            await res.Negotiate(result);
        }

        private async Task SearchPurchaseOrders(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource> purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.Search(searchTerm, req.HttpContext.GetPrivileges());
            await res.Negotiate(result);
        }

        private async Task SearchTariffs(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<Tariff, int, TariffResource, TariffResource> tariffService)
        {
            var result = tariffService.Search(searchTerm);

            await res.Negotiate(result);
        }

        private async Task UpdatePurchaseOrder(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrderResource resource,
            IFacadeResourceService<PurchaseOrder, int, PurchaseOrderResource, PurchaseOrderResource> purchaseOrderFacadeService)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = purchaseOrderFacadeService.Update(resource.OrderNumber, resource, privileges, res.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }
    }
}
