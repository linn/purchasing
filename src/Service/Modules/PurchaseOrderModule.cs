namespace Linn.Purchasing.Service.Modules
{
    using System.IO;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PurchaseOrderModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
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

            app.MapGet("/purchasing/purchase-orders/reqs", this.SearchReqs);
            app.MapGet("/purchasing/purchase-orders/reqs/states", this.GetReqStates);
            app.MapGet("/purchasing/purchase-orders/reqs/print", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/reqs/{id:int}/print", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/reqs/create", this.GetApp);

            app.MapGet("/purchasing/purchase-orders/reqs/application-state", this.GetReqApplicationState);
            app.MapGet("/purchasing/purchase-orders/reqs/{id:int}", this.GetReq);
            app.MapPut("/purchasing/purchase-orders/reqs/{id:int}", this.UpdateReq);
            app.MapPost("/purchasing/purchase-orders/reqs/{id:int}/cancel", this.CancelReq);
            app.MapPost("/purchasing/purchase-orders/reqs/email", this.EmailReq);
            app.MapPost("/purchasing/purchase-orders/reqs/email-for-authorisation", this.EmailForReqAuthorisation);
            app.MapPost("/purchasing/purchase-orders/reqs/email-for-finance", this.EmailForReqFinanceCheck);
            app.MapPost("/purchasing/purchase-orders/reqs/{id:int}/authorise", this.AuthoriseReq);
            app.MapPost("/purchasing/purchase-orders/reqs", this.CreateReq);
        }

        private async Task AuthoriseReq(
            HttpRequest req, 
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.Authorise(
                id,
                req.HttpContext.GetPrivileges(),
                req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task CancelReq(
            HttpRequest req, 
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.DeleteOrObsolete(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task EmailReq(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var toEmailAddress = req.Query.As<string>("toEmailAddress");

            using var ms = new MemoryStream();

            await req.Body.CopyToAsync(ms);
            var result = purchaseOrderReqFacadeService.SendEmail(
                req.HttpContext.User.GetEmployeeNumber(),
                toEmailAddress,
                reqNumber,
                ms);

            await res.Negotiate(result);
        }

        private async Task EmailForReqAuthorisation(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            int toEmployeeId,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.SendAuthorisationRequestEmail(
                req.HttpContext.User.GetEmployeeNumber(),
                toEmployeeId,
                reqNumber);

            await res.Negotiate(result);
        }

        private async Task EmailForReqFinanceCheck(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            int toEmployeeId,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.SendFinanceCheckRequestEmail(
                req.HttpContext.User.GetEmployeeNumber(),
                toEmployeeId,
                reqNumber);

            await res.Negotiate(result);
        }

        private async Task CreateReq(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrderReqResource resource,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.Add(resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
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

        private async Task GetReq(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService
            )
        {
            var result = purchaseOrderReqFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetReqApplicationState(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            await res.Negotiate(purchaseOrderReqFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task GetReqStates(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<PurchaseOrderReqState, string, PurchaseOrderReqStateResource, PurchaseOrderReqStateResource> purchaseOrderReqStateService)
        {
            var result = purchaseOrderReqStateService.GetAll();

            await res.Negotiate(result);
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

        private async Task SearchReqs(
            HttpRequest req,
            HttpResponse res,
            string reqNumber,
            string part,
            string supplier,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.FilterBy(
                new PurchaseOrderReqSearchResource
                    {
                        ReqNumber = reqNumber, Part = part, Supplier = supplier
                    },
                req.HttpContext.GetPrivileges());

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
            resource.Privileges = req.HttpContext.GetPrivileges();

            var result = purchaseOrderFacadeService.Update(resource.OrderNumber, resource, resource.Privileges, res.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task UpdateReq(
            HttpRequest req,
            HttpResponse res,
            int id,
            PurchaseOrderReqResource resource,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.Update(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
