namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class EdiModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/edi", this.GetApp);
            app.MapGet("/purchasing/edi/suppliers", this.GetEdiSuppliers);
            app.MapGet("/purchasing/edi/orders", this.GetEdiOrders);
            app.MapPost("/purchasing/edi/orders", this.SendEdiOrder);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetEdiOrders(
            HttpRequest req,
            HttpResponse res,
            int supplierId,
            IEdiOrdersFacadeService ediOrdersFacadeService)
        {
            var result = ediOrdersFacadeService.GetEdiOrders(supplierId);

            await res.Negotiate(result);
        }

        private async Task GetEdiSuppliers(
            HttpRequest req,
            HttpResponse res,
            IEdiOrdersFacadeService ediOrdersFacadeService)
        {
            var result = ediOrdersFacadeService.GetEdiSuppliers();

            await res.Negotiate(result);
        }

        private async Task SendEdiOrder(
            HttpRequest req,
            HttpResponse res,
            SendEdiEmailResource resource,
            IEdiOrdersFacadeService ediOrdersFacadeService)
        {
            var result = ediOrdersFacadeService.SendEdiOrder(resource);

            await res.Negotiate(result);
        }
    }
}
