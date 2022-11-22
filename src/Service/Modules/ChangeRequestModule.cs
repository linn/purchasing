namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ChangeRequestModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/change-requests", this.GetApp);
            app.MapGet("/purchasing/change-requests/{id:int}", this.GetChangeRequest);
            app.MapPost("/purchasing/change-requests/approve", this.ApproveChangeRequest);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetChangeRequest(
            HttpRequest req,
            HttpResponse res,
            int id,
            IChangeRequestFacadeService facadeService)
        {
            var result = facadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task ApproveChangeRequest(
            HttpRequest req,
            HttpResponse res,
            ChangeRequestRequestResource request,
            IChangeRequestFacadeService facadeService)
        {
            var result = facadeService.ApproveChangeRequest(request.Id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
