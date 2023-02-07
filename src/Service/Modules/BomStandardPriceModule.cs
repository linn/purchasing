namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class BomStandardPriceModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/boms/prices", this.Search);
            app.MapGet("/purchasing/boms/standards-set", this.GetApp);
            app.MapPost("/purchasing/boms/prices", this.UpdateStandardPrices);
        }

        private async Task Search(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IBomStandardPriceFacadeService service)
        {
            var result = service.GetData(searchTerm);

            await res.Negotiate(result);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task UpdateStandardPrices(
            HttpRequest req,
            HttpResponse res,
            BomStandardPricesResource resource,
            IBomStandardPriceFacadeService service)
        {
            resource.UpdatedBy = req.HttpContext.User.GetEmployeeNumber();
            var result = service.DoUpdate(resource);
            await res.Negotiate(result);
        }
    }
}
