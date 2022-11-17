namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class BomTypeChangeModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/bom-type-change", this.GetApp);
            app.MapGet("/purchasing/bom-type-change/{id}", this.GetBomTypeChange);
            app.MapPost("/purchasing/bom-type-change", this.PostBomTypeChange);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetBomTypeChange(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPartFacadeService partFacadeService)
        {
            var result = partFacadeService.GetBomType(
                id,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task PostBomTypeChange(
            HttpRequest req,
            HttpResponse res,
            BomTypeChangeResource resource,
            IPartFacadeService partFacadeService)
        {
            resource.ChangedBy = req.HttpContext.User.GetEmployeeNumber();
            var result = partFacadeService.ChangeBomType(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
