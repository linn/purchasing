namespace Linn.Purchasing.Service.Modules
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

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
            app.MapPost("/purchasing/change-requests", this.CreateChangeRequest);
            app.MapGet("/purchasing/change-requests/create", this.GetApp);
            app.MapGet("/purchasing/change-requests", this.GetChangeRequests);
            app.MapGet("/purchasing/change-requests/{id:int}", this.GetChangeRequest);
            app.MapPost("/purchasing/change-requests/status", this.ChangeStatus);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetChangeRequests(
            HttpRequest req, 
            HttpResponse res,
            IChangeRequestFacadeService facadeService,
            string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
            }

            await res.Negotiate(facadeService.Search(searchTerm));
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

        private async Task ChangeStatus(
            HttpRequest req,
            HttpResponse res,
            ChangeRequestStatusChangeResource request,
            IChangeRequestFacadeService facadeService)
        {
            var result = facadeService.ChangeStatus(request, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateChangeRequest(
            HttpRequest req,
            HttpResponse res,
            ChangeRequestResource request,
            IChangeRequestFacadeService facadeService)
        {
            var result = facadeService.Add(request, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
