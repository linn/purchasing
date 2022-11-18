namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Common.Facade.Carter.Extensions;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Facade.Services;
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
            app.MapGet("/purchasing/boms/tree", this.GetTree);
            app.MapGet("/purchasing/boms/tree/options", this.GetApp);
            app.MapGet("/purchasing/boms/{id:int}", this.GetBom);
            app.MapGet("/purchasing/boms/boards/application-state", this.GetBoardApplicationState);
            app.MapGet("/purchasing/boms/boards/{id}", this.GetBoard);
            app.MapGet("/purchasing/boms/tree/export", this.GetTreeExport);
            app.MapGet("/purchasing/boms/boards", this.GetBoards);
            app.MapGet("/purchasing/boms/boards/create", this.GetApp);
            app.MapPost("/purchasing/boms/boards", this.AddCircuitBoard);
            app.MapPut("/purchasing/boms/boards/{id}", this.UpdateCircuitBoard);
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

        private async Task GetTree(
            HttpRequest req,
            HttpResponse res,
            string bomName,
            int? levels,
            bool requirementOnly,
            bool showChanges,
            string treeType,
            IBomTreeReportsService facadeService)
        {
            IResult<BomTreeNode> result = null;
            if (!string.IsNullOrEmpty(bomName))
            {
                result = facadeService.GetTree(bomName.Trim().ToUpper(), levels, requirementOnly, showChanges, treeType);
            }

            await res.Negotiate(result);
        }

        private async Task GetTreeExport(
            HttpRequest req,
            HttpResponse res,
            string bomName,
            int? levels,
            bool requirementOnly,
            bool showChanges,
            string treeType,
            IBomTreeReportsService facadeService)
        {
            var result = facadeService.GetFlatTreeExport(
                bomName.Trim().ToUpper(), levels, requirementOnly, showChanges, treeType);

            await res.FromCsv(result, $"{bomName}.csv");
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
        
        private async Task UpdateCircuitBoard(
            HttpRequest req,
            HttpResponse res,
            string id,
            CircuitBoardResource resource,
            IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource> circuitBoardFacadeService)
        {
            var result = circuitBoardFacadeService.Update(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetBoardApplicationState(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource> circuitBoardFacadeService)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = circuitBoardFacadeService.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task AddCircuitBoard(
            HttpRequest req,
            HttpResponse res,
            CircuitBoardResource resource,
            IFacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardResource> circuitBoardFacadeService)
        {
            var result = circuitBoardFacadeService.Add(
                resource,
                req.HttpContext.GetPrivileges(),
                null);

            await res.Negotiate(result);
        }
    }
}
