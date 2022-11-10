namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class BomModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/boms/tree/{id:int}", this.GetApp);
            app.MapGet("/purchasing/boms/{id:int}", this.GetBom);
            app.MapGet("/purchasing/boms/boards/{id}", this.GetBoard);
            app.MapGet("/purchasing/boms/boards", this.GetBoards);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetBom(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceService<Bom, int, BomResource, BomResource> facadeService)
        {
            var result = facadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetBoard(
            HttpRequest req,
            HttpResponse res,
            string id,
            IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource> circuitBoardFacadeService)
        {
            var result = circuitBoardFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetBoards(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource> circuitBoardFacadeService)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var result = circuitBoardFacadeService.GetAll();
                await res.Negotiate(result);
            }
            else
            {
                var searchResult = circuitBoardFacadeService.Search(searchTerm);
                await res.Negotiate(searchResult);
            }
        }
    }
}
