namespace Linn.Purchasing.Service.Modules
{
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
            app.MapGet("/purchasing/change-requests/replace", this.GetApp);
            app.MapGet("/purchasing/change-requests", this.GetChangeRequests);
            app.MapGet("/purchasing/change-requests/{id:int}", this.GetChangeRequest);
            app.MapPost("/purchasing/change-requests/status", this.ChangeStatus);
            app.MapPost("/purchasing/change-requests/phase-ins", this.PostPhaseIns);
            app.MapPost("/purchasing/change-requests/replace", this.ChangeRequestReplace);
            app.MapPut("/purchasing/change-requests/{id:int}", this.UpdateChangeRequest);
            app.MapGet("/purchasing/change-requests/pcas-component-changes", this.GetPcasChangeComponents);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetChangeRequests(
            HttpRequest req, 
            HttpResponse res,
            IChangeRequestFacadeService facadeService,
            string searchTerm,
            bool? includeAllForBom,
            bool? includeForBoard,
            bool? outstanding,
            int? lastMonths,
            bool? cancelled,
            string boardCode = null,
            string rootProduct = null)
        {
            if (string.IsNullOrEmpty(searchTerm) && outstanding == null)
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
                return;
            }

            if (includeAllForBom.GetValueOrDefault())
            {
                await res.Negotiate(facadeService.GetChangeRequestsRelevantToBom(searchTerm));
                return;
            }

            if (includeForBoard.GetValueOrDefault())
            {
                await res.Negotiate(facadeService.GetChangeRequestsRelevantToBoard(searchTerm));
                return;
            }

            await res.Negotiate(facadeService.SearchChangeRequests(searchTerm, outstanding, lastMonths, cancelled, boardCode, rootProduct));
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
            var employeeId = req.HttpContext.User.GetEmployeeNumber();
            var result = facadeService.ChangeStatus(request, employeeId, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task PostPhaseIns(
            HttpRequest req,
            HttpResponse res,
            ChangeRequestPhaseInsResource request,
            IChangeRequestFacadeService facadeService)
        {
            var result = facadeService.PhaseInChangeRequest(request, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task ChangeRequestReplace(
            HttpRequest req,
            HttpResponse res,
            ChangeRequestReplaceResource request,
            IChangeRequestFacadeService facadeService)
        {
            var employeeId = req.HttpContext.User.GetEmployeeNumber();
            var result = facadeService.ChangeRequestReplace(request, employeeId, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateChangeRequest(
            HttpRequest req,
            HttpResponse res,
            ChangeRequestResource request,
            IChangeRequestFacadeService facadeService)
        {
            var employeeId = req.HttpContext.User.GetEmployeeNumber();
            var result = facadeService.AddAndReplace(request, employeeId, req.HttpContext.GetPrivileges());
            await res.Negotiate(result);
        }

        private async Task UpdateChangeRequest(
            HttpRequest req,
            HttpResponse res,
            int id,
            ChangeRequestResource resource,
            IChangeRequestFacadeService facadeService)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = facadeService.Update(
                id,
                resource,
                privileges,
                res.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task GetPcasChangeComponents(
            HttpRequest request,
            HttpResponse response,
            int documentNumber,
            IPcasChangeComponentsService service)
        {
            var result = service.GetChanges(documentNumber);

            await response.Negotiate(result);
        }
    }
}
