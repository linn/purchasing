namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.MaterialRequirements;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class MaterialRequirementsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/material-requirements/last-run", this.GetDetails);
            app.MapGet("/purchasing/material-requirements/run-logs", this.GetAllRunLogs);
            app.MapGet("/purchasing/material-requirements/run-logs/{id:int}", this.GetRunLogById);
            app.MapGet("/purchasing/material-requirements/run-mrp", this.GetApp);
            app.MapPost("/purchasing/material-requirements/run-mrp", this.RunMrp);

            app.MapGet("/purchasing/material-requirements/used-on-report", this.GetUsedOnReport);
         
            app.MapGet("/purchasing/material-requirements", this.GetApp);
            app.MapGet("/purchasing/material-requirements/report", this.GetApp);
            app.MapPost("/purchasing/material-requirements", this.GetMaterialRequirements);
            app.MapGet("/purchasing/material-requirements/options", this.GetMaterialRequirementsOptions);
        }

        private async Task RunMrp(
            HttpRequest request,
            HttpResponse response,
            IMaterialRequirementsPlanningFacadeService materialRequirementsPlanningFacadeService)
        {
            await response.Negotiate(materialRequirementsPlanningFacadeService.RunMrp(request.HttpContext.GetPrivileges()));
        }

        private async Task GetRunLogById(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource> mrpRunLogFacadeService)
        {
            var result = mrpRunLogFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetAllRunLogs(
            HttpRequest req,
            HttpResponse res,
            string jobRef,
            IMaterialRequirementsPlanningFacadeService materialRequirementsPlanningFacadeService,
            IFacadeResourceFilterService<MrpRunLog, int, MrpRunLogResource, MrpRunLogResource, MaterialRequirementsSearchResource> mrpRunLogFacadeService)
        {
            if (string.IsNullOrEmpty(jobRef))
            {
                var result = mrpRunLogFacadeService.GetAll(req.HttpContext.GetPrivileges());
                await res.Negotiate(result);
            }
            else
            {
                var result = mrpRunLogFacadeService.FindBy(
                    new MaterialRequirementsSearchResource { JobRef = jobRef },
                    req.HttpContext.GetPrivileges());
                await res.Negotiate(result);
            }
        }

        private async Task GetDetails(
            HttpRequest req,
            HttpResponse res,
            ISingleRecordFacadeResourceService<MrMaster, MrMasterResource> masterFacadeService)
        {
            var result = masterFacadeService.Get(req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetUsedOnReport(
            HttpRequest req,
            HttpResponse res,
            IMrUsedOnReportFacadeService service,
            string partNumber)
        {
            var result = service.GetReport(partNumber);
            await res.Negotiate(result);
        }
        
        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetMaterialRequirements(
            HttpRequest req,
            HttpResponse res,
            IMaterialRequirementsReportFacadeService facadeService,
            MrRequestResource request)
        {
            var result = facadeService.GetMaterialRequirements(request, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetMaterialRequirementsOptions(
            HttpRequest req,
            HttpResponse res,
            IMaterialRequirementsReportFacadeService facadeService)
        {
            var result = facadeService.GetOptions(req.HttpContext.GetPrivileges());
            await res.Negotiate(result);
        }
    }
}
