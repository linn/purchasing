﻿namespace Linn.Purchasing.Service.Modules
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Rendering;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
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
            app.MapGet("/purchasing/purchase-orders/application-state", this.GetApplicationState);
            app.MapGet("/purchasing/purchase-orders/allow-over-book", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/quick-create", this.GetApp);

            app.MapGet("/purchasing/purchase-orders/{orderNumber:int}/allow-over-book/", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/create", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/currencies", this.GetCurrencies);
            app.MapGet("/purchasing/purchase-orders/methods", this.GetOrderMethods);
            app.MapGet("/purchasing/purchase-orders/delivery-addresses", this.GetDeliveryAddresses);
            app.MapGet("/purchasing/purchase-orders/units-of-measure", this.GetUnitsOfMeasure);
            app.MapGet("/purchasing/purchase-orders/packaging-groups", this.GetPackagingGroups);
            app.MapGet("/purchasing/purchase-orders/tariffs", this.SearchTariffs);
            app.MapGet("/purchasing/purchase-orders", this.SearchPurchaseOrders);
            app.MapPost(
                "/purchasing/purchase-orders/generate-order-from-supplier-id", 
                this.FillOutPurchaseOrderFromSupplierId);
            app.MapGet("/purchasing/purchase-orders/{orderNumber:int}", this.GetPurchaseOrder);
            app.MapGet("/purchasing/purchase-orders/{orderNumber:int}/html", this.GetPurchaseOrderHtml);
            app.MapPost("/purchasing/purchase-orders/email-pdf", this.EmailOrderPdf);
            app.MapPost("/purchasing/purchase-orders/email-supplier-ass", this.EmailSupplierAss);
            app.MapGet("/purchasing/purchase-orders/auth-or-send", this.GetApp);
            app.MapPost("/purchasing/purchase-orders/authorise-multiple", this.AuthorisePurchaseOrders);
            app.MapPost("/purchasing/purchase-orders/email-multiple", this.EmailPurchaseOrders);
            app.MapPut("/purchasing/purchase-orders/{orderNumber:int}", this.UpdatePurchaseOrder);
            app.MapPost("/purchasing/purchase-orders", this.CreateOrder);
            app.MapPost("/purchasing/purchase-orders/email-for-authorisation", this.EmailFinanceForAuthorisation);
            app.MapPost("/purchasing/purchase-orders/{id:int}/authorise", this.AuthoriseOrder);
            app.MapPost("/purchasing/purchase-orders/{orderNumber:int}/email-dept", this.EmailDept);
            app.MapPost("/purchasing/purchase-orders/{orderNumber:int}/switch-our-qty-price", this.SwitchOurQtyPrice);
            app.MapPatch("/purchasing/purchase-orders/{orderNumber:int}", this.PatchOrder);
        }

        private async Task AuthoriseOrder(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.AuthorisePurchaseOrder(
                id, req.HttpContext.GetPrivileges(), req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task CreateOrder(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrderResource resource,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.Add(resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetApplicationState(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            await res.Negotiate(purchaseOrderFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task AuthorisePurchaseOrders(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrdersProcessRequestResource requestResource,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.AuthorisePurchaseOrders(
                requestResource,
                req.HttpContext.GetPrivileges(),
                req.HttpContext.User.GetEmployeeNumber());
            await res.Negotiate(result);
        }

        private async Task EmailPurchaseOrders(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrdersProcessRequestResource requestResource,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.EmailOrderPdfs(
                requestResource,
                req.HttpContext.GetPrivileges(),
                req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task EmailFinanceForAuthorisation(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderFacadeService purchaseOrderFacadeService,
            int orderNumber)
        {
            var result = purchaseOrderFacadeService.EmailFinanceAuthRequest(
                req.HttpContext.User.GetEmployeeNumber(), orderNumber);

            await res.Negotiate(result);
        }

        private async Task EmailDept(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderFacadeService purchaseOrderFacadeService,
            int orderNumber)
        {
            var result = purchaseOrderFacadeService.EmailDept(
                orderNumber,
                req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task SwitchOurQtyPrice(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderFacadeService purchaseOrderFacadeService,
            int orderNumber,
            int orderLine)
        {
            var result = purchaseOrderFacadeService.SwitchOurQtyPrice(
                orderNumber,
                orderLine,
                req.HttpContext.User.GetEmployeeNumber(),
                req.HttpContext.GetPrivileges()?.ToList());

            await res.Negotiate(result);
        }

        private async Task PatchOrder(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderFacadeService purchaseOrderFacadeService,
            PatchRequestResource<PurchaseOrderResource> resource)
        {
            var result = purchaseOrderFacadeService.PatchOrder(
                resource,
                req.HttpContext.User.GetEmployeeNumber(),
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
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
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.GetById(orderNumber, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task FillOutPurchaseOrderFromSupplierId(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrderResource resource,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.FillOutOrderFromSupplierId(
                resource, req.HttpContext.GetPrivileges(), req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task GetPurchaseOrderHtml(
            HttpRequest req,
            HttpResponse res,
            int orderNumber,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.GetOrderAsHtml(orderNumber);

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(result);
        }

        private async Task GetNoteHtml(
            HttpRequest req,
            HttpResponse res,
            int noteNumber,
            IRepository<PlCreditDebitNote, int> repo,
            IHtmlTemplateService<PlCreditDebitNote> templateService)
        {
            var result = repo.FindById(noteNumber);
            var html = await templateService.GetHtml(result);
            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;
        
            await res.WriteAsync(html);
        }

        private async Task EmailOrderPdf(
            HttpRequest req,
            HttpResponse res,
            int orderNumber,
            string emailAddress,
            bool bcc,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.EmailOrderPdf(
                orderNumber,
                emailAddress,
                bcc,
                req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task EmailSupplierAss(
            HttpRequest req,
            HttpResponse res,
            int orderNumber,
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var result = purchaseOrderFacadeService.EmailSupplierAss(
                             orderNumber);

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
            string startDate,
            string endDate,
            IPurchaseOrderFacadeService purchaseOrderFacadeService,
            int numberToTake = 50)
        {
            var resource = new PurchaseOrderSearchResource
                               {
                                   OrderNumber = searchTerm,
                                   StartDate = startDate,
                                   EndDate = endDate,
                                   SearchTerm = searchTerm
                               };
            var result = purchaseOrderFacadeService.FilterBy(resource, numberToTake, req.HttpContext.GetPrivileges());
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
            IPurchaseOrderFacadeService purchaseOrderFacadeService)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = purchaseOrderFacadeService.Update(
                resource.OrderNumber,
                resource,
                privileges,
                res.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }
    }
}
