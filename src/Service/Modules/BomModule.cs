namespace Linn.Purchasing.Service.Modules
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Common.Reporting.Resources.ReportResultResources;
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
            app.MapGet("/purchasing/boms/cost/options", this.GetApp);
            app.MapGet("/purchasing/boms", this.GetApp);
            app.MapGet("/purchasing/boms/bom-utility", this.GetApp);

            app.MapGet("/purchasing/boms/boards/application-state", this.GetBoardApplicationState);
            app.MapGet("/purchasing/boms/boards/{id}", this.GetBoard);
            app.MapGet("/purchasing/boms/board-components/{id}", this.GetBoard);
            app.MapGet("/purchasing/boms/tree/flat", this.GetFlatTree);
            app.MapGet("/purchasing/boms/boards", this.GetBoards);
            app.MapGet("/purchasing/boms/board-components", this.GetBoards);
            app.MapGet("/purchasing/boms/boards-summary", this.GetBoardsSummary);
            app.MapGet("/purchasing/boms/boards/create", this.GetApp);
            app.MapPost("/purchasing/boms/boards", this.AddCircuitBoard);
            app.MapPut("/purchasing/boms/boards/{id}", this.UpdateCircuitBoard);
            app.MapPut("/purchasing/boms/board-components/{id}", this.UpdateBoardComponents);

            app.MapGet("/purchasing/boms/reports/list", this.GetPartsOnBomReport);

            app.MapGet("/purchasing/boms/reports/cost/options", this.GetApp);
            app.MapGet("/purchasing/boms/reports/cost", this.GetBomCostReport);
            app.MapGet("/purchasing/boms/reports/cost/pdf", this.GetBomCostReportPdf);

            app.MapPost("/purchasing/boms/tree", this.PostBomTree);

            app.MapPost("/purchasing/boms/copy", this.CopyBom);
            app.MapPost("/purchasing/boms/delete", this.DeleteAllFromBom);
            app.MapPost("/purchasing/boms/explode", this.ExplodeSubAssembly);
            app.MapPost("/purchasing/purchase-orders/boms/upload-board-file", this.UploadBoardFile);
            app.MapPost("/purchasing/purchase-orders/boms/upload-smt-file", this.UploadSmtFile);
            app.MapGet("/purchasing/boms/board-components-smt-check", this.GetApp);

            app.MapGet("/purchasing/boms/reports/bom-print", this.GetBomPrintReport);
        }

        private async Task GetApp(HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
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
                result = facadeService.GetTree(
                    bomName.Trim().ToUpper(), 
                    levels, 
                    requirementOnly, 
                    showChanges, 
                    treeType);
            }

            await res.Negotiate(result);
        }

        private async Task GetFlatTree(
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
            
            await res.Negotiate(result);
        }

        private async Task GetBoard(
            HttpRequest req,
            HttpResponse res,
            string id,
            ICircuitBoardFacadeService circuitBoardFacadeService)
        {
            var result = circuitBoardFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetBoardsSummary(
            HttpRequest req,
            HttpResponse res,
            string boardCode,
            string revisionCode,
            string cref,
            string partNumber,
            IQueryFacadeResourceService<BoardComponentSummary, BoardComponentSummaryResource, BoardComponentSummaryResource> boardComponentSummaryFacadeService)
        {
            var searchResource = new BoardComponentSummaryResource
                                     {
                                         BoardCode = boardCode?.ToUpper(),
                                         RevisionCode = revisionCode?.ToUpper(),
                                         Cref = cref?.ToUpper(),
                                         PartNumber = partNumber?.ToUpper()
            };
            var result = boardComponentSummaryFacadeService.FilterBy(searchResource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetBoards(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            ICircuitBoardFacadeService circuitBoardFacadeService)
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
            CircuitBoardComponentsUpdateResource resource,
            ICircuitBoardFacadeService circuitBoardFacadeService)
        {
            var result = circuitBoardFacadeService.Update(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task UpdateBoardComponents(
            HttpRequest req,
            HttpResponse res,
            string id,
            CircuitBoardComponentsUpdateResource resource,
            ICircuitBoardFacadeService circuitBoardFacadeService)
        {
            resource.UserNumber = req.HttpContext.User.GetEmployeeNumber();
            var result = circuitBoardFacadeService.UpdateBoardComponents(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
        
        private async Task GetBoardApplicationState(
            HttpRequest req,
            HttpResponse res,
            ICircuitBoardFacadeService circuitBoardFacadeService)
        {
            var privileges = req.HttpContext.GetPrivileges();

            var result = circuitBoardFacadeService.GetApplicationState(privileges);

            await res.Negotiate(result);
        }

        private async Task AddCircuitBoard(
            HttpRequest req,
            HttpResponse res,
            CircuitBoardResource resource,
            ICircuitBoardFacadeService circuitBoardFacadeService)
        {
            var result = circuitBoardFacadeService.Add(
                resource,
                req.HttpContext.GetPrivileges(),
                null);

            await res.Negotiate(result);
        }

        private async Task PostBomTree(
            HttpRequest req,
            HttpResponse res,
            PostBomResource resource,
            IBomFacadeService bomFacadeService)
        {
            resource.EnteredBy = req.HttpContext.User.GetEmployeeNumber();
            var result = bomFacadeService.PostBom(
                resource);

            await res.Negotiate(result);
        }

        private async Task GetPartsOnBomReport(
            HttpRequest req,
            HttpResponse res,
            string bomName,
            IBomReportsFacadeService facadeService)
        {
            IResult<ReportReturnResource> result = null;
            if (!string.IsNullOrEmpty(bomName))
            {
                result = facadeService.GetPartsOnBomReport(bomName);
            }

            await res.Negotiate(result);
        }

        private async Task GetBomCostReport(
            HttpRequest req,
            HttpResponse res,
            string bomName,
            bool splitBySubAssembly,
            int levels,
            decimal labourHourlyRate,
            IBomReportsFacadeService facadeService)
        {
            IResult<IEnumerable<BomCostReportResource>> result = null;
            if (!string.IsNullOrEmpty(bomName))
            {
                result = facadeService.GetBomCostReport(
                    bomName, splitBySubAssembly, levels, labourHourlyRate);
            }

            await res.Negotiate(result);
        }

        private async Task GetBomCostReportPdf(
            HttpRequest req,
            HttpResponse res,
            string bomName,
            bool splitBySubAssembly,
            int levels,
            decimal labourHourlyRate,
            IBomReportsFacadeService facadeService)
        {
            await res.FromStream(
                facadeService.GetBomCostReportPdf(bomName, splitBySubAssembly, levels, labourHourlyRate),
                "application.pdf",
                new System.Net.Mime.ContentDisposition("attachment"));
        }

        private async Task CopyBom(
            HttpRequest req,
            HttpResponse res,
            BomFunctionResource functionResource,
            IBomFacadeService bomFacadeService)
        {
            var result = bomFacadeService.CopyBom(
                functionResource.SrcPartNumber, 
                functionResource.DestPartNumber, 
                req.HttpContext.User.GetEmployeeNumber(), 
                functionResource.CrfNumber,
                functionResource.AddOrOverwrite,
                functionResource.RootName);

            await res.Negotiate(result);
        }

        private async Task DeleteAllFromBom(
            HttpRequest req,
            HttpResponse res,
            BomFunctionResource functionResource,
            IBomFacadeService bomFacadeService)
        {
            var result = bomFacadeService.DeleteBom(
                functionResource.DestPartNumber,
                functionResource.CrfNumber,
                req.HttpContext.User.GetEmployeeNumber(),
                functionResource.RootName);

            await res.Negotiate(result);
        }

        private async Task ExplodeSubAssembly(
            HttpRequest req,
            HttpResponse res,
            BomFunctionResource functionResource,
            IBomFacadeService bomFacadeService)
        {
            var result = bomFacadeService.ExplodeSubAssembly(
                functionResource.DestPartNumber,
                functionResource.CrfNumber,
                functionResource.SubAssembly,
                req.HttpContext.User.GetEmployeeNumber(),
                functionResource.RootName);

            await res.Negotiate(result);
        }

        private async Task UploadBoardFile(
            HttpRequest req,
            HttpResponse res,
            ICircuitBoardFacadeService circuitBoardFacadeService,
            string boardCode,
            string revisionCode,
            int? changeRequestId,
            bool makeChanges)
        {
            IResult<ProcessResultResource> result;

            if (req.ContentType == "text/tab-separated-values")
            {
                var reader = new StreamReader(req.Body).ReadToEndAsync();

                result = circuitBoardFacadeService.UploadBoardFile(
                    boardCode,
                    revisionCode,
                    "TSB",
                    reader.Result,
                    changeRequestId,
                    makeChanges,
                    req.HttpContext.GetPrivileges());
            }
            else
            {
                result = new BadRequestResult<ProcessResultResource>("Unsupported content type.");
            }

            await res.Negotiate(result);
        }

        private async Task UploadSmtFile(
            HttpRequest req,
            HttpResponse res,
            ICircuitBoardFacadeService circuitBoardFacadeService,
            string boardCode,
            string revisionCode,
            int? changeRequestId)
        {
            IResult<ProcessResultResource> result;

            if (req.ContentType == "text/tab-separated-values")
            {
                var reader = new StreamReader(req.Body).ReadToEndAsync();

                result = circuitBoardFacadeService.UploadBoardFile(
                    boardCode,
                    revisionCode,
                    "SMT",
                    reader.Result,
                    changeRequestId,
                    false,
                    req.HttpContext.GetPrivileges());
            }
            else
            {
                result = new BadRequestResult<ProcessResultResource>("Unsupported content type.");
            }

            await res.Negotiate(result);
        }

        private async Task GetBomPrintReport(
            HttpResponse res,
            IBomReportsFacadeService service,
            string bomName)
        {
            if (string.IsNullOrEmpty(bomName))
            {
                await this.GetApp(res);
            }
            else
            {
                await res.Negotiate(service.GetBomPrintReport(bomName));
            }
        }
    }
}
