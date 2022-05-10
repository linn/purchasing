namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PurchaseOrderDeliveryModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/purchase-orders/deliveries", this.Search);
            app.MapPatch("/purchasing/purchase-orders/deliveries/{orderNumber:int}/{orderLine:int}/{deliverySeq:int}", this.Patch);
        }

        private async Task Search(
            HttpRequest req,
            HttpResponse res,
            string supplierSearchTerm,
            string orderNumberSearchTerm,
            bool includeAcknowledged,
            IPurchaseOrderDeliveryFacadeService service)
        {
            var result = service.SearchDeliveries(supplierSearchTerm, orderNumberSearchTerm, includeAcknowledged);
            await res.Negotiate(result);
        }

        private async Task Patch(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrderDeliveryPatchRequestResource resource,
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
    }
}
