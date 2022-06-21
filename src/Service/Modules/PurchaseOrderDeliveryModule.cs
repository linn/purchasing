namespace Linn.Purchasing.Service.Modules
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Common.Serialization.Json;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PurchaseOrderDeliveryModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/purchase-orders/acknowledge", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/deliveries", this.Search);
            app.MapPatch("/purchasing/purchase-orders/deliveries/{orderNumber:int}/{orderLine:int}/{deliverySeq:int}", this.Patch);
            app.MapPost("/purchasing/purchase-orders/deliveries", this.BatchUpdate);
            app.MapPost("/purchasing/purchase-orders/deliveries/{orderNumber:int}/{orderLine:int}/", this.UpdateDeliveriesForOrderLine);
            app.MapGet("/purchasing/purchase-orders/deliveries/{orderNumber:int}/{orderLine:int}/", this.GetDeliveriesForOrderLine);
        }

        private async Task Search(
            HttpRequest req,
            HttpResponse res,
            string supplierSearchTerm,
            string orderNumberSearchTerm,
            bool includeAcknowledged,
            IPurchaseOrderDeliveryFacadeService service)
        {
            var result = service.SearchDeliveries(
                supplierSearchTerm, 
                orderNumberSearchTerm, 
                includeAcknowledged);
            await res.Negotiate(result);
        }

        private async Task GetDeliveriesForOrderLine(
            HttpRequest req,
            HttpResponse res,
            int orderNumber,
            int orderLine,
            IPurchaseOrderDeliveryFacadeService service)
        {
            var result = service.GetDeliveriesForDetail(orderNumber, orderLine);
            await res.Negotiate(result);
        }

        private async Task Patch(
            HttpRequest req,
            HttpResponse res,
            PatchRequestResource<PurchaseOrderDeliveryResource> resource,
            int orderNumber,
            int orderLine,
            int deliverySeq,
            IPurchaseOrderDeliveryFacadeService service)
        {
            var result = service.PatchDelivery(
                new PurchaseOrderDeliveryKey
                    {
                        OrderNumber = orderNumber,
                        OrderLine = orderLine,
                        DeliverySequence = deliverySeq
                    }, 
                resource, 
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task BatchUpdate(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderDeliveryFacadeService service)
        {
            IResult<BatchUpdateProcessResultResource> result;

            if (req.Headers.ContentType == "text/csv")
            {
                var reader = new StreamReader(req.Body).ReadToEndAsync();

                result = service.BatchUpdateDeliveries(
                    reader.Result,
                    req.HttpContext.GetPrivileges());
            }
            else if (req.ContentType == "application/json")
            {
                var resource = await req.Bind<IEnumerable<PurchaseOrderDeliveryUpdateResource>>();
                
                result = service.BatchUpdateDeliveries(
                    resource, 
                    req.HttpContext.GetPrivileges());
            }
            else
            {
                result = new BadRequestResult<BatchUpdateProcessResultResource>("Unsupported content type.");
            }
            
            await res.Negotiate(result);
        }

        private async Task UpdateDeliveriesForOrderLine(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrderDeliveryResource[] resource,
            int orderNumber,
            int orderLine,
            IPurchaseOrderDeliveryFacadeService service)
        {
            var result = service.UpdateDeliveriesForDetail(
                orderNumber,
                orderLine,
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }
    }
}
