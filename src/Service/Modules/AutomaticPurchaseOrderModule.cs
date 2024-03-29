﻿namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class AutomaticPurchaseOrderModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/automatic-purchase-orders", this.GetAutomaticPurchaseOrders);
            app.MapGet("/purchasing/automatic-purchase-orders/{id:int}", this.GetAutomaticPurchaseOrderById);
            app.MapGet("/purchasing/automatic-purchase-orders/application-state", this.GetApplicationState);
            app.MapPost("/purchasing/automatic-purchase-orders", this.CreateAutomaticPurchaseOrder);
            app.MapPut("/purchasing/automatic-purchase-orders/{id:int}", this.UpdateAutomaticPurchaseOrder);
            app.MapGet("/purchasing/automatic-purchase-order-suggestions", this.GetAutomaticPurchaseOrderSuggestions);
        }

        private async Task GetAutomaticPurchaseOrders(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource> automaticPurchaseOrderFacadeService)
        {
            await res.Negotiate(automaticPurchaseOrderFacadeService.GetAll());
        }

        private async Task GetApplicationState(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource> automaticPurchaseOrderFacadeService)
        {
            await res.Negotiate(automaticPurchaseOrderFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task GetAutomaticPurchaseOrderById(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource> automaticPurchaseOrderFacadeService)
        {
            var result = automaticPurchaseOrderFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateAutomaticPurchaseOrder(
            HttpRequest request,
            HttpResponse response,
            IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource> automaticPurchaseOrderFacadeService,
            AutomaticPurchaseOrderResource resource)
        {
            var result = automaticPurchaseOrderFacadeService.Add(
                resource,
                request.HttpContext.GetPrivileges());

            await response.Negotiate(result);
        }

        private async Task UpdateAutomaticPurchaseOrder(
            HttpRequest request,
            HttpResponse response,
            int id,
            AutomaticPurchaseOrderResource resource,
            IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource> automaticPurchaseOrderFacadeService)
        {
            var result = automaticPurchaseOrderFacadeService.Update(
                id,
                resource,
                request.HttpContext.GetPrivileges(),
                request.HttpContext.User.GetEmployeeNumber());

            await response.Negotiate(result);
        }

        private async Task GetAutomaticPurchaseOrderSuggestions(
            HttpRequest req,
            HttpResponse res,
            int? supplierId,
            int? planner,
            IFacadeResourceFilterService<AutomaticPurchaseOrderSuggestion, int, AutomaticPurchaseOrderSuggestionResource, AutomaticPurchaseOrderSuggestionResource, PlannerSupplierRequestResource> automaticPurchaseOrderSuggestionFacadeService)
        {
            await res.Negotiate(
                automaticPurchaseOrderSuggestionFacadeService.FilterBy(
                    new PlannerSupplierRequestResource { Planner = planner, SupplierId = supplierId }));
        }
    }
}
